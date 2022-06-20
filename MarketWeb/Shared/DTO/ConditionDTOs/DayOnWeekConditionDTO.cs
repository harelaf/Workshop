﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MarketWeb.Shared.DTO
{
    public class DayOnWeekConditionDTO : IConditionDTO
    {
        private DayOfWeek _dayOnWeek;
        private bool _negative;

        public String DayOnWeek => _dayOnWeek.ToString();
        public bool Negative => _negative;
        public int ObjType { get => 1; set { return; } }
        public DayOnWeekConditionDTO(String dayOnWeek, bool negative)
        {
            _dayOnWeek = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), dayOnWeek);
            _negative = negative;
        }
    }
}
