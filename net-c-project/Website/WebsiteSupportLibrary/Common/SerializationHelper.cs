using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace WebsiteSupportLibrary.Common
{
    public static class SerializationHelper
    {
        public static void ObjectFlatCopy<T, K>(T from, K to)
        {
            System.Reflection.FieldInfo[] fromFields = from.GetType().GetFields(
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (System.Reflection.FieldInfo fi in fromFields)
            {
                fi.SetValue(to, fi.GetValue(from));
            }
        }


        public static T JsonToObject<T>(this string jsonObj, int recursionDepth = 100)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Deserialize<T>(jsonObj);
        }

        public static string ObjectToJson<T>(T obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();            
            String json = serializer.Serialize(obj);
            return json;
        }
    }
}