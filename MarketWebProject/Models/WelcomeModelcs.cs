namespace MarketWebProject.Models
{
    public class WelcomeModelcs
    {
        public string? RequestId { get; set; }

        public string userName { get; set; }
        public string password { get; set; }
        public DateTime DOB { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
