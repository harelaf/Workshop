﻿using MarketProject.Service;
namespace MarketWebProject.Views.Shared
{
    public static class LayoutConfig
    {
        public static bool IsGuest { get; set; } = true;
        public static bool IsLoggedIn { get; set; } = false;
        public static bool IsAdmin { get; set; } = false;
        //public static MarketAPI _service { get; set; } = new MarketAPI();



    }
}
