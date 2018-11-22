using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaeClass.Helper
{
    public class JsonHelper
    {
        public static string SerializeObject(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return JsonConvert.SerializeObject(value);
        }

        public static string Remove(string jsonData, string propertyName, ref string propertyValue)
        {
            JToken jsonToken = JToken.Parse(jsonData);
            if (jsonToken == null)
                throw new InvalidOperationException("Invalid json data format!");

            var targetToken = jsonToken.Children<JProperty>().FirstOrDefault(p => p.Name == propertyName);
            if (targetToken != null)
            {
                propertyValue = targetToken.Value.ToString(Newtonsoft.Json.Formatting.None);
                targetToken.Remove();
            }

            return jsonToken.ToString();
        }

        public static List<TModel> ConvertToModelList<TModel>(List<string> jsonDataList) where TModel : class
        {
            List<TModel> res = new List<TModel>();
            foreach (var jsonData in jsonDataList)
            {
                res.Add(JsonConvert.DeserializeObject<TModel>(jsonData));
            }
            
            return res;
        }

        public static TModel ConvertToModel<TModel>(string jsonData) where TModel : class
        {
            if (string.IsNullOrEmpty(jsonData))
                throw new ArgumentNullException("jsonData");

            return JsonConvert.DeserializeObject<TModel>(jsonData);
        }

        public static string JsonToXml(string json)
        {
            try
            {
                var xmlDoc = JsonConvert.DeserializeXmlNode(json, "ContentData");

                return xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

    }
}
