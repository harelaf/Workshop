using MarketWeb.Shared.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
namespace MarketWeb.Shared
{
    public class ItemConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(AtomicDiscountDTO);
        public override bool CanWrite => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);

            string type = (string)obj["$type"];
            if (type != null && type.Contains(nameof(ItemDiscountDTO)))
            {
                return serializer.Deserialize<ItemDiscountDTO>(reader);
            }
            return null;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    public class CategoryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(AtomicDiscountDTO);
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return serializer.Deserialize<CategoryDiscountDTO>(reader);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value, typeof(CategoryDiscountDTO));
        }
    }
}