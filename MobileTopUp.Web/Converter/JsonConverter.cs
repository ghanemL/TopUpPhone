using Newtonsoft.Json;

namespace MobileTopUp.Web.Converter
{
    public class JsonConverter : IJsonConverter
    {
        public T Deserialize<T>(Stream responseContent)
        {
            using var sr = new StreamReader(responseContent);
            using var reader = new JsonTextReader(sr);
            var serializer = new JsonSerializer();

            return serializer.Deserialize<T>(reader);
        }
    }
}
