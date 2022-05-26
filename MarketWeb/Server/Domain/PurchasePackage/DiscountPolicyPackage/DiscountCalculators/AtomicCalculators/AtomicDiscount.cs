﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Server.Domain.PurchasePackage.DiscountPackage
{
    public abstract class AtomicDiscount : Discount
    {
        private DateTime _expiration;
        public override DateTime GetExpirationDate(ISearchablePriceable searchablePriceable)
        {
            return _expiration;
        }
        protected String ExpirationToString(int indent)
        { 
            return newLine(indent) + "Expired on: " + _expiration.ToString();
        }
        protected AtomicDiscount(DiscountCondition condition, DateTime expiration) : base(condition)
        {
            _expiration = expiration;
        }

        protected AtomicDiscount(DateTime expiration) : base()
        {
            _expiration = expiration;
        }
    }
}
