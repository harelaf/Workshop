using MarketProject.Domain.PurchasePackage.DiscountPackage;
using System;
using System.Collections.Generic;

namespace MarketProject.Service.DTO
{
    public class dtoConditionConverter
    {
        

        ////////////  add all convertion methods for each condition  /////////////

        //public Condition convertConcrete(AndCompositionDTO condition_dto)
        //{
        //    bool negative = condition_dto.Negative;
        //    List<Condition> conditions = new List<Condition>();
        //    foreach (IConditionDTO cond in condition_dto.Conditions)
        //        conditions.Add(convertCondition(cond));
        //    return new AndComposition(negative, conditions);
        //}
        //public Condition convertConcrete(DayOnWeekConditionDTO condition_dto)
        //{
        //    String day = condition_dto.DayOnWeek;
        //    bool negative = condition_dto.Negative;
        //    return new DayOnWeekCondition(day, negative);
        //}
        //public Condition convertConcrete(HourConditionDTO condition_dto)
        //{
        //    int minHour = condition_dto.MinHour;
        //    int maxHour = condition_dto.MaxHour;
        //    bool negative = condition_dto.Negative;
        //    return new HourCondition(minHour, maxHour, negative);
        //}
        //public Condition convertConcrete(OrCompositionDTO condition_dto)
        //{
        //    bool negative = condition_dto.Negative;
        //    List<Condition> conditions = new List<Condition>();
        //    foreach (IConditionDTO cond in condition_dto.Conditions)
        //        conditions.Add(convertCondition(cond));
        //    return new OrComposition(negative, conditions);
        //}
        //public Condition convertConcrete(PriceableConditionDTO condition_dto)
        //{
        //    String keyWord = condition_dto.KeyWord;
        //    int minAmount = condition_dto.MinAmount;
        //    int maxAmount = condition_dto.MaxAmount;
        //    bool negative = condition_dto.Negative;
        //    return new PriceableCondition(keyWord, minAmount, maxAmount, negative);
        //}
        //public Condition convertConcrete(SearchCategoryConditionDTO condition_dto)
        //{
        //    String keyWord = condition_dto.KeyWord;
        //    int minAmount = condition_dto.MinAmount;
        //    int maxAmount = condition_dto.MaxAmount;
        //    bool negative = condition_dto.Negative;
        //    return new SearchCategoryCondition(keyWord, minAmount, maxAmount, negative);
        //}
        //public Condition convertConcrete(SearchItemConditionDTO condition_dto)
        //{
        //    String keyWord = condition_dto.KeyWord;
        //    int minAmount = condition_dto.MinAmount;
        //    int maxAmount = condition_dto.MaxAmount;
        //    bool negative = condition_dto.Negative;
        //    return new SearchItemCondition(keyWord, minAmount, maxAmount, negative);
        //}
        //public Condition convertConcrete(XorCompositionDTO condition_dto)
        //{
        //    bool negative = condition_dto.Negative;
        //    List<Condition> conditions = new List<Condition>();
        //    foreach (IConditionDTO cond in condition_dto.Conditions)
        //        conditions.Add(convertCondition(cond));
        //    return new XorComposition(negative, conditions);
        //}
    }
}