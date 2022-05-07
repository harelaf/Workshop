namespace MarketWebProject.Models
{
    public class MainModel
    {
        public bool ErrorOccurred { get; set; }
        public String? Message { get; set; }

        public bool IsGuest { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsAdmin { get; set; }

        public MainModel()
        {
            ErrorOccurred = false;
            Message = null;
            IsGuest = true;
            IsLoggedIn = false;
            IsAdmin = false;

        }
        public MainModel(bool ErrorOccurred, string err, bool isGuest, bool isLoggedIn, bool isAdmin)
        {
            this.ErrorOccurred = ErrorOccurred;
            Message = err;
            IsGuest = true;
            IsLoggedIn = false;
            IsAdmin = false;
        }
        public MainModel(bool isGuest, bool isLoggedIn , bool isAdmin)
        {
            ErrorOccurred = false;
            Message = null;
            IsGuest = isGuest;
            IsLoggedIn = isLoggedIn;
            IsAdmin = isAdmin;
        }


    }
}
