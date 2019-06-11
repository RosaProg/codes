using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteSupportLibrary.Common
{
    //public static class AppHelper
    //{
    //    public struct ValuesOption
    //    {
    //        public string Text;
    //        public string Action;
    //    }

    //    public class MyDictionary : Dictionary<int, ValuesOption>
    //    {
    //        public void Add(int key, string text, string action)
    //        {
    //            ValuesOption val;
    //            val.Text = text;
    //            val.Action = action;
    //            this.Add(key, val);
    //        }
    //    }

    //    public static QuestionnaireText AddTextToSection(QuestionnaireSection section, string actionId, string text)
    //    {
    //        QuestionnaireText txt = new QuestionnaireText()
    //        {
    //            ActionId = actionId,
    //            OrderInSection = section.Elements.Count + 1,                
    //            Text = text
    //        };
    //        section.Elements.Add(txt);

    //        return txt;
    //    }

    //    public static QuestionnaireItem AddItemToSection(QuestionnaireSection section, string actionId, string displayId, string text)
    //    {
    //        QuestionnaireItem item = new QuestionnaireItem()
    //        {
    //            ActionId = actionId,
    //            DisplayId = displayId,
    //            OrderInSection = section.Elements.Count + 1,                
    //            Text = text
    //        };
    //        section.Elements.Add(item);

    //        return item;
    //    }

    //    public static QuestionnaireItemOptionGroup AddOptionGroupToItem(QuestionnaireItem item, MyDictionary options, int rangeStep, QuestionnaireResponseType type)
    //    {
    //        QuestionnaireItemOptionGroup qog = new QuestionnaireItemOptionGroup();
    //        qog.OrderInItem = item.OptionGroups.Count + 1;
    //        qog.RangeStep = rangeStep;
    //        qog.ResponseType = type;
    //        foreach (int v in options.Keys)
    //        {
    //            qog.Options.Add(new QuestionnaireItemOption() { Text = options[v].Text, Value = v, Action = options[v].Action });
    //        }
            
    //        item.OptionGroups.Add(qog);

    //        return qog;
    //    }
    //}
}