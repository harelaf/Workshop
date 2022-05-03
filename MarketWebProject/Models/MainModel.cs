namespace MarketWebProject.Models
{
    public class MainModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public String? ErrorMsg { get; set; }

        public bool IsGuest { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsAdmin { get; set; }

        public MainModel()
        {
            ErrorMsg = null;
            IsGuest = true;
            IsLoggedIn = false;
            IsAdmin = false;

        }
        public MainModel(string err)
        {
            ErrorMsg = err;
            IsGuest = true;
            IsLoggedIn = false;
            IsAdmin = false;
        }
        public MainModel(bool isGuest, bool isLoggedIn , bool isAdmin)
        {
            ErrorMsg = null;
            IsGuest = isGuest;
            IsLoggedIn = isLoggedIn;
            IsAdmin = isAdmin;
        }


    }
}
