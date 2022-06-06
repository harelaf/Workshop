using MarketWeb.Shared.DTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace MarketWeb.Shared
{
    public class DiscountSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(AtomicDiscountDTO).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
    public class ConditionSpecifiedConcreteClassConverter : DefaultContractResolver
    {
        protected override JsonConverter ResolveContractConverter(Type objectType)
        {
            if (typeof(IConditionDTO).IsAssignableFrom(objectType) && !objectType.IsAbstract)
                return null; // pretend TableSortRuleConvert is not specified (thus avoiding a stack overflow)
            return base.ResolveContractConverter(objectType);
        }
    }
    public class DiscountConverter : JsonConverter
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new DiscountSpecifiedConcreteClassConverter() };
        public override bool CanConvert(Type objectType) => objectType == typeof(AtomicDiscountDTO);
        public override bool CanWrite => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            switch (obj["ObjType"].Value<int>())
            {
                case 1:
                    return JsonConvert.DeserializeObject<AllProductsDiscountDTO>(obj.ToString(), settings);
                case 2:
                    return JsonConvert.DeserializeObject<CategoryDiscountDTO>(obj.ToString(), settings);
                case 3:
                    return JsonConvert.DeserializeObject<ItemDiscountDTO>(obj.ToString(), settings);
                case 4:
                    return JsonConvert.DeserializeObject<NumericDiscountDTO>(obj.ToString(), settings);
                default:
                    return null;

            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    public class ConditionConverter : JsonConverter
    {
        static JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new ConditionSpecifiedConcreteClassConverter() };
        public override bool CanConvert(Type objectType) => objectType == typeof(IConditionDTO);
        public override bool CanWrite => false;
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            switch (obj["ObjType"].Value<int>())
            {
                case 0:
                    return JsonConvert.DeserializeObject<AndCompositionDTO>(obj.ToString(), settings);
                case 1:
                    String day = obj["DayOnWeek"].Value<String>();
                    bool Negative = obj["Negative"].Value<bool>();
                    return new DayOnWeekConditionDTO(day, Negative);
                case 2:
                    return JsonConvert.DeserializeObject<HourConditionDTO>(obj.ToString(), settings);
                case 3:
                    return JsonConvert.DeserializeObject<OrCompositionDTO>(obj.ToString(), settings);
                case 4:
                    return JsonConvert.DeserializeObject<PriceableConditionDTO>(obj.ToString(), settings);
                case 5:
                    return JsonConvert.DeserializeObject<SearchCategoryConditionDTO>(obj.ToString(), settings);
                case 6:
                    return JsonConvert.DeserializeObject<SearchItemConditionDTO>(obj.ToString(), settings);
                case 7:
                    return JsonConvert.DeserializeObject<XorCompositionDTO>(obj.ToString(), settings);
                default:
                    return null;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    //public class DetailsConverter : JsonConverter
    //{
    //    //static JsonSerializerSettings settings = new JsonSerializerSettings { ContractResolver = new ConditionSpecifiedConcreteClassConverter() };
    //    public override bool CanConvert(Type objectType) => objectType == typeof(DiscountDetailsDTO);
    //    public override bool CanWrite => false;
    //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //    {
    //        JObject obj = JObject.Load(reader);
    //        int amount = obj["Amount"].Value<int>();
    //        double price = obj["ActualPrice"].Value<double>();
    //        List<String> actualDisList = obj["DiscountList"].ToObject<List<String>>();
    //        return new DiscountDetailsDTO(amount, actualDisList, price);
    //    }
    //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}