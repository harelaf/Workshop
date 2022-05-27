﻿namespace ClientWebApplication.Views.Shared
{
    public static class LayoutConfig
    {
        public static bool IsGuest { get; set; } = true;
        public static bool IsLoggedIn { get; set; } = false;
        public static bool IsAdmin { get; set; } = false;
    }
}
