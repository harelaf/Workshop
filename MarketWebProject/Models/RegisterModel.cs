namespace MarketWebProject.Models
{
    public class RegisterModel
    {
        public String? ErrorMsg { get; set; }

        public RegisterModel()
        {
            ErrorMsg = null;
        }
        public RegisterModel(string err)
        {
            ErrorMsg = err;
        }
    }
}
