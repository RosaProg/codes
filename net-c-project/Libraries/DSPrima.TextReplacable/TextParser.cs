using DSPrima.TextReplaceable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DSPrima.TextReplaceable
{
    /// <summary>
    /// Parses a given piece of text and replaces all replaceable codes with the appropriate value
    /// </summary>
    /// <typeparam name="TKey">An Enum that defines the key for the IReplaceable</typeparam>
    public class TextParser<TKey> where TKey : IComparable, IFormattable, IConvertible
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextParser{TKey}"/> class
        /// </summary>
        public TextParser()
        {
            if (!typeof(TKey).IsEnum) throw new ArgumentException("T must be an enumerated type");
        }

        /// <summary>
        /// Retrieves the message of the given Message code and replaces all ReplacementCodes with the proper value
        /// </summary>
        /// <param name="text">The text to parse</param>
        /// <param name="replaceable">Any objects that are available should be passed on as they may be used as part of the parsing process.</param>
        /// <returns>The text element parsed</returns>
        public string ParseMessage(string text, IReplaceable<TKey> replaceable)
        {
            if (string.IsNullOrWhiteSpace(text)) return string.Empty;

            StringBuilder message = new StringBuilder();
            List<IReplaceableCode<TKey>> codes = replaceable.GetReplaceableCodes();
            Dictionary<TKey, object> objectsToParse = replaceable.GetReplaceables();

            return this.ParseText(text, codes, objectsToParse);
        }

        /// <summary>
        /// Parses the text with the given list of replaceable codes and objects to get the data from
        /// </summary>
        /// <param name="textToParse">The text to parse</param>
        /// <param name="codes">The codes to use for parsing</param>
        /// <param name="objectsToParse">The objects to parse</param>
        /// <returns>The parsed string</returns>6
        private string ParseText(string textToParse, List<IReplaceableCode<TKey>> codes, Dictionary<TKey, object> objectsToParse)
        {
            StringBuilder text = new StringBuilder(textToParse);

            List<string> codesReplaced = new List<string>();
            List<string> codesNotFound = new List<string>();

            foreach (IReplaceableCode<TKey> code in codes)
            {
                if (/*text.Contains(code.ReplacementCode) &&*/ !codesReplaced.Contains(code.ReplacementCode))
                {
                    string replacementValue = this.FindValue(code, objectsToParse);
                    if (replacementValue != null)
                    {
                        text.Replace(code.ReplacementCode, replacementValue);
                        codesReplaced.Add(code.ReplacementCode);
                        codesNotFound.Remove(code.ReplacementCode);
                    }
                    else
                    {
                        codesNotFound.Add(code.ReplacementCode);
                    }
                }
            }

            foreach (string code in codesNotFound)
            {
                text.Replace(code, string.Empty);
            }

            return text.ToString();
        }

        /// <summary>
        /// Finds a value in a list of objects to parse for a given list of codes
        /// </summary>
        /// <param name="code">The code to find the value for</param>
        /// <param name="objectsToParse">The object to look in</param>
        /// <returns>The value found or null if nothing is found</returns>
        private string FindValue(IReplaceableCode<TKey> code, Dictionary<TKey, object> objectsToParse)
        {
            if (code.UseReplacementValue)
            {
                return code.ReplacementValue;
            }
            else if (objectsToParse.ContainsKey(code.ObjectKey) && objectsToParse[code.ObjectKey] != null)
            {
                string[] variablePath = code.ObjectVariablePath.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (variablePath.Length == 0)
                {
                    return objectsToParse[code.ObjectKey].ToString();
                }
                else
                {
                    string result = this.FindValue(objectsToParse[code.ObjectKey], variablePath, code.ToStringParameter);
                    if (result != null) return result;
                }
            }

            return null;
        }

        /// <summary>
        /// Finds a value starting at the given object and walking down the variable path
        /// If the path is empty, the toString version of the current object is user instead.
        /// </summary>
        /// <param name="o">The object to look for the variable for</param>
        /// <param name="variablePath">The path of variables to follow</param>
        /// <param name="toStringParameter">A single parameter for the ToString function</param>
        /// <returns>The found value or null if nothing is found</returns>
        private string FindValue(object o, string[] variablePath, string toStringParameter)
        {            
            object result = null;

            if (variablePath.Length > 0)
            {
                Type type = o.GetType();
                PropertyInfo prop = type.GetProperty(variablePath[0].Trim());
                if (prop != null)
                {
                    result = prop.GetValue(o);
                    if (variablePath.Length > 1)
                    {
                        return this.FindValue(result, new ArraySegment<string>(variablePath, 1, variablePath.Length - 1).ToArray(), toStringParameter);
                    }
                }
            }
            else
            {
                result = o;
            }

            if (result != null)
            {
                if (!string.IsNullOrEmpty(toStringParameter))
                {
                    MethodInfo mi = result.GetType().GetMethods().Where((m) =>
                    {
                        if (m.Name != "ToString") return false;
                        var parameters = m.GetParameters();
                        if (parameters.Length != 1) return false;
                        if (parameters[0].ParameterType != typeof(string)) return false;

                        return true;
                    }).FirstOrDefault();

                    if (mi != null) return mi.Invoke(result, new object[] { toStringParameter }).ToString();
                }
                else
                {
                    return result.ToString();
                }
            }

            return null;
        }
    }
}
