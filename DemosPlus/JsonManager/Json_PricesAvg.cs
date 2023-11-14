using DemosPlus.Url;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemosPlus.Json
{
    [JsonConverter(typeof(JsonPathConverter))]
    public class JsonPricesAvg
    {
        [JsonProperty("location")]
        public City city { get; set; }

        [JsonProperty("item_id")]
        public Item item { get; set; }

        [JsonProperty("quality")]
        public int quality { get; set; }

        [JsonProperty("data.prices_avg")]
        public List<double> prices_avg { get; set; }
    }

    [JsonConverter(typeof(JsonPathConverter))]
    public class JsonBuyMaxPrices
    { 
    
    }

    public partial class JsonManager
    {
        public static List<JsonPricesAvg> GetPricesAvg(string json)
        {
            return JsonConvert.DeserializeObject<List<JsonPricesAvg>>(json);
        }

        public static List<>

    }

    public class JsonPathConverter : JsonConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var targetObj = Activator.CreateInstance(objectType);

            foreach (var prop in objectType.GetProperties().Where(p => p.CanRead && p.CanWrite))
            {
                var att = prop.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();

                var jsonPath = (att != null ? att.PropertyName : prop.Name);
                var token = jo.SelectToken(jsonPath);

                if (token == null || token.Type == JTokenType.Null) continue;

                var value = token.ToObject(prop.PropertyType, serializer);
                prop.SetValue(targetObj, value, null);
            }

            return targetObj;
        }

        public override bool CanConvert(Type objectType) => false;

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}
