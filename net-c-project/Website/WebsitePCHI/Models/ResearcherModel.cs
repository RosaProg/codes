using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Script.Serialization;

namespace WebsitePCHI.Models
{
    public class ResearcherModel
    {
        public List<string> PatientFields { get; set; }
        public List<string> QuestionnaireFields { get; set; }
    }

    [KnownType(typeof(group))]
    [KnownType(typeof(condition))]
    public class SearchObject
    {
        public string templateName { get; set; }
    }

    public class group : SearchObject
    {
        public List<SearchObject> children { get; set; }
        public List<string> logicalOperators { get; set; }
        public string selectedLogicalOperator { get; set; }        

        public group()
        {
            this.children = new List<SearchObject>();
            this.logicalOperators = new List<string>();
        }
    }

    public class condition : SearchObject
    {
        public List<string> classType { get; set; }
        public string selectedClass { get; set; }
        public bool ShowPrimaryFields { get; set; }
        public bool ShowSecondaryFields { get; set; }
        public List<string> fields { get; set; }
        public string selectedField { get; set; }
        public List<string> comparisons { get; set; }
        public string selectedComparison { get; set; }
        public string value { get; set; }

        public condition()
        {
            this.classType = new List<string>();
            this.fields = new List<string>();
            this.comparisons = new List<string>();
        }
    }

    public class ResearcherModelResolver : SimpleTypeResolver
    {
        public override Type ResolveType(string id)
        {
            switch(id)
            {
                case "condition" :
                    return typeof(condition);                    
                case "group":
                    return typeof(group);
            }
            return base.ResolveType(id);
        }
    }
}