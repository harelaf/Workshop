namespace MarketWebProject.Models
{
    public class MainModel
    {
        public bool ErrorOccurred { get; set; }
        public String? ErrorMsg { get; set; }

        public bool IsGuest { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsAdmin { get; set; }

        public MainModel()
        {
            ErrorOccurred = false;
            ErrorMsg = null;
            IsGuest = true;
            IsLoggedIn = false;
            IsAdmin = false;

        }
        public MainModel(bool ErrorOccurred, string err)
        {
            this.ErrorOccurred = ErrorOccurred;
            ErrorMsg = err;
            IsGuest = true;
            IsLoggedIn = false;
            IsAdmin = false;
        }
        public MainModel(bool isGuest, bool isLoggedIn , bool isAdmin)
        {
            ErrorOccurred = false;
            ErrorMsg = null;
            IsGuest = isGuest;
            IsLoggedIn = isLoggedIn;
            IsAdmin = isAdmin;
        }


    }
}
