using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiTestFramework.Helpers
{
    public static class JsonHelper
    {
        public static T? Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }

        public static bool ValidateJsonSchema(string json, string expectedProperty)
        {
            var jObject = JObject.Parse(json);
            return jObject[expectedProperty] != null;
        }

        public static string GetJsonValue(string json, string path)
        {
            var jObject = JObject.Parse(json);
            var token = jObject.SelectToken(path);
            return token?.ToString() ?? string.Empty;
        }
    }
}
