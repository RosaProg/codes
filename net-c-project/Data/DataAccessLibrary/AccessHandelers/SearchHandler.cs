using LinqKit;
using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Questionnaire.Pro;
using PCHI.Model.Questionnaire.Response;
using PCHI.Model.Research;
using PCHI.Model.Users;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PCHI.DataAccessLibrary.AccessHandelers
{
    /// <summary>
    /// Handlers the special user defined searches
    /// </summary>
    public class SearchHandler
    {
        /// <summary>
        /// The Main Database context to use
        /// </summary>
        private MainDatabaseContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchHandler"/> class
        /// </summary>
        /// <param name="context">The <see cref="MainDatabaseContext"/> instance to use</param>
        internal SearchHandler(MainDatabaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Searchers the QuestionnaireUserGroups with the given parameters
        /// </summary>
        /// <param name="group">The root of the search parameters</param>
        /// <returns>A list of all the QuestionnaireUserResponseGroups that match the search parameters</returns>
        public List<QuestionnaireUserResponseGroup> SearchQuestionnaireUserResponseGroups(SearchGroup group)
        {
            var pr = this.BuildResponseGroupQuery(group);
            pr = pr.And(r => this.context.Questionnaires.OfType<ProInstrument>().Select(q => q.Id).Contains(r.Questionnaire.Id));

            var query = this.context.QuestionnaireUserResponseGroups.Include(r => r.Questionnaire).Include(r => r.Responses.Select(resp => resp.Item.OptionGroups)).Include(r => r.Responses.Select(resp => resp.Option.Group.Item)).Include(r => r.Patient).Include(r => r.ProDomainResultSet.Select(s => s.Results.Select(sr => sr.Domain.ResultRanges))).AsExpandable();
            query = query.Where(r => r.Completed).Where(pr.Expand());

            var result = query.ToList();

            return result;
        }

        /// <summary>
        /// Builds a response query for a QuestionnaireUserResponseGroup
        /// </summary>
        /// <param name="group">The Search group that contains the search parameters</param>
        /// <returns>The expression that comprises the logic for the query</returns>
        public Expression<Func<QuestionnaireUserResponseGroup, bool>> BuildResponseGroupQuery(SearchGroup group)
        {
            var pr = PredicateBuilder.False<QuestionnaireUserResponseGroup>();
            if (group.IsAndOperator) pr = PredicateBuilder.True<QuestionnaireUserResponseGroup>();
            foreach (var child in group.Children)
            {
                if (child == null) continue;
                Expression<Func<QuestionnaireUserResponseGroup, bool>> query = null;
                if (child.GetType() == typeof(SearchGroup))
                {
                    query = this.BuildResponseGroupQuery((SearchGroup)child);
                }
                else
                {
                    query = this.BuildConditionQuery((SearchCondition)child);
                }

                switch (group.IsAndOperator)
                {
                    case true:
                        pr = pr.And(query);
                        break;
                    case false:
                        pr = pr.Or(query);
                        break;
                }
            }

            return pr.Expand();
        }

        /// <summary>
        /// Builds a conditional expression for a QuestionnaireUserResponseGroup
        /// </summary>
        /// <param name="searchCondition">The SearchCondition to create the expression for</param>
        /// <returns>The requested expression</returns>
        private Expression<Func<QuestionnaireUserResponseGroup, bool>> BuildConditionQuery(SearchCondition searchCondition)
        {
            Expression<Func<QuestionnaireUserResponseGroup, bool>> result = null;
            if (searchCondition.SearchType == typeof(ProInstrument))
            {
                result = BuildCondition<QuestionnaireUserResponseGroup, string>(r => r.Questionnaire.Name, searchCondition.Value, searchCondition.Comparison);
            }
            else if (searchCondition.SearchType == typeof(Patient))
            {
                SearchPatient patient = (SearchPatient)searchCondition;
                var expression = BuildCondition<QuestionnaireUserResponseGroupTag, string>(t => t.TextValue, searchCondition.Value, searchCondition.Comparison, t2 => t2.TagName == patient.TagName);
                result = r => this.context.QuestionnaireUserResponseGroupTags.Where(expression).Where(t => t.GroupId == r.Id).Count() > 0;
            }
            else if (searchCondition.SearchType == typeof(QuestionnaireUserResponseGroup))
            {
                SearchResponseGroup group = (SearchResponseGroup)searchCondition;
                DateTime value;
                if (!DateTime.TryParse(group.Value, out value))
                {
                    throw new ArgumentException("Value [" + group.Value + "] is not a valid date time variable");
                }

                value = new DateTime(value.Year, value.Month, value.Day);
                switch (group.SearchField)
                {
                    case SearchResponseGroupFields.DateTimeStarted:
                        result = this.BuildCondition<QuestionnaireUserResponseGroup, DateTime>(r => r.StartTime.Value, value, group.Comparison);
                        break;
                    case SearchResponseGroupFields.DateTimeCompleted:
                        result = this.BuildCondition<QuestionnaireUserResponseGroup, DateTime>(r => r.DateTimeCompleted.Value, value, group.Comparison);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Builds a conditional Expression based upon the comparison type passed
        /// </summary>
        /// <typeparam name="T">The class the conditional expression is for</typeparam>
        /// <typeparam name="DataType">The type of data to be compared upon</typeparam>
        /// <param name="selector">Expression to retrieve the Field to compare on</param>
        /// <param name="value">The value to compare to</param>
        /// <param name="comparison">The comparison type to use</param>
        /// <param name="expressions">Any additional expression to add (using AND) to this one)</param>
        /// <returns>The expression requested</returns>
        private Expression<Func<T, bool>> BuildCondition<T, DataType>(Expression<Func<T, DataType>> selector, DataType value, Comparison comparison, params Expression<Func<T, bool>>[] expressions)
        {
            Expression<Func<T, bool>> result = null;
            switch (comparison)
            {
                case Comparison.Equals:
                default:
                    if (typeof(DataType) == typeof(DateTime))
                    {
                        result = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(value)), selector.Parameters);
                        DateTime d2 = ((DateTime)(object)value).AddDays(1);
                        result = result.And(Expression.Lambda<Func<T, bool>>(Expression.LessThan(selector.Body, Expression.Constant(d2)), selector.Parameters));
                    }
                    else
                    {
                        result = Expression.Lambda<Func<T, bool>>(Expression.Equal(selector.Body, Expression.Constant(value)), selector.Parameters);
                    }

                    break;
                case Comparison.NotEquals:
                    if (typeof(DataType) == typeof(DateTime))
                    {
                        result = Expression.Lambda<Func<T, bool>>(Expression.LessThan(selector.Body, Expression.Constant(value)), selector.Parameters);
                        DateTime d2 = ((DateTime)(object)value).AddDays(1);
                        result = result.Or(Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(d2)), selector.Parameters));
                    }
                    else
                    {
                        result = Expression.Lambda<Func<T, bool>>(Expression.NotEqual(selector.Body, Expression.Constant(value)), selector.Parameters);
                    }

                    break;
                case Comparison.SmallerThan:
                    result = Expression.Lambda<Func<T, bool>>(Expression.LessThan(selector.Body, Expression.Constant(value)), selector.Parameters);
                    break;
                case Comparison.SmallerOrEquals:
                    if (typeof(DataType) == typeof(DateTime))
                    {
                        DateTime d2 = ((DateTime)(object)value).AddDays(1);
                        result = Expression.Lambda<Func<T, bool>>(Expression.LessThan(selector.Body, Expression.Constant(d2)), selector.Parameters);
                    }
                    else
                    {
                        result = Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(selector.Body, Expression.Constant(value)), selector.Parameters);
                    }

                    break;
                case Comparison.GreaterThan:
                    if (typeof(DataType) == typeof(DateTime))
                    {
                        DateTime d2 = ((DateTime)(object)value).AddDays(1);
                        result = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(d2)), selector.Parameters);
                    }
                    else
                    {
                        result = Expression.Lambda<Func<T, bool>>(Expression.GreaterThan(selector.Body, Expression.Constant(value)), selector.Parameters);
                    }

                    break;
                case Comparison.GreaterOrEquals:
                    result = Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(selector.Body, Expression.Constant(value)), selector.Parameters);
                    break;
            }

            if (expressions != null)
            {
                foreach (var exp in expressions) result = result.And(exp);
            }

            return result.Expand();
        }
    }
}
