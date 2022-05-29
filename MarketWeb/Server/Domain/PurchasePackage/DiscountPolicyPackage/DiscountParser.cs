using MarketWeb.Server.Domain.PurchasePackage.DiscountPackage;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPolicyPackage
{
    public class DiscountParser
    {
        private string DiscountString;
        private string ConditionString;

        public DiscountParser(string discountString, string conditionString)        {
            DiscountString = discountString;
            ConditionString = conditionString;
        }

        public Discount parse()
        {
            DiscountCondition conditionParsed = parseCondition();
            return parseDiscount(conditionParsed);
        }

        private DiscountCondition parseCondition()
        {
            return null;
        }

        private Discount parseDiscount(DiscountCondition condition)
        {
            return null;
        }
    }
}
