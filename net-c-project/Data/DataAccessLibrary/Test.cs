using PCHI.DataAccessLibrary.Context;
using PCHI.Model.Questionnaire.Pro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PCHI.DataAccessLibrary
{
    public class Test
    {
        public static void TestThis()
        {/*
            MainDatabaseContext context = new MainDatabaseContext();
            string questionnarieName = "OES2";
            var query = (from g in context.QuestionnaireUserResponseGroups.Where(r => r.Questionnaire.Name == questionnarieName)
                         join q in context.Questionnaires.OfType<ProInstrument>().Where(q2 => q2.Name == questionnarieName) on g.Questionnaire equals q
                         join d in context.ProDomains on q.Id equals d.Instrument.Id 
                         join r in context.ProDomainResultRanges on d.Id equals r.Domain.Id
                         select new { g, q, d, r });//.Include(q2 => q2.q2.Domains.Select(d => d.ResultRanges));//.Select(g2 => g2);
            var query2 = context.QuestionnaireUserResponseGroups.Where(r=>r.Questionnaire.Name == questionnarieName).
                Select()

            var step1 = query.ToList();
            step1.ForEach(g => g.g.Questionnaire = g.q);
            var result = step1.Select(g => g.g);
          */
        }
    }
}
