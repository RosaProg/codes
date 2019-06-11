using NCalc;
using PCHI.Model.Questionnaire.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace PCHI.BusinessLogic.DomainScoreEngine
{
    /// <summary>
    /// Contains logic for scoring a forumula
    /// </summary>
    public class DomainScoreEngine
    {
        /// <summary>
        /// Replace all the given questionNumbers in the proCalculation string with the actual values of
        /// the patient answers
        /// </summary>
        /// <param name="patientAnswers">List of patientAnswers which contains the ActionId and the value of the answer</param>
        /// <param name="domainFormula">String which contains the formula of the PRO domain to be calculated</param>
        /// <returns>the result of the calculation after replacing all the ActionIds with the correct values</returns>
        public static double CalculateResult(List<QuestionnaireResponse> patientAnswers, string domainFormula)
        {
            StringBuilder builderProCalculation = new StringBuilder(domainFormula);
            foreach (QuestionnaireResponse answer in patientAnswers)
            {
                // Each questionIdentifier in the proCalculation string should be enclosed by {}
                // to identify clearly each question
                // Example: {1} this means that the question identifier in this example is 1
                // Example: {Q1} this means that the question identifier in this example is Q1
                builderProCalculation.Replace("{" + answer.Item.ActionId + "}", answer.ResponseValue.ToString());
            }

            // The new expression contains all the value of every question in the proCalculation string
            try
            {
                Expression expression = new Expression(builderProCalculation.ToString());
                return double.Parse(expression.Evaluate().ToString());
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}