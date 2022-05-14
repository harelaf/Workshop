namespace MarketWebProject.Models
{
    public class MainModel
    {
        public bool ErrorOccurred { get; set; }
        public String? Message { get; set; }

        public MainModel()
        {
            ErrorOccurred = false;
            Message = null;
        }
        public MainModel(bool ErrorOccurred, string err)
        {
            this.ErrorOccurred = ErrorOccurred;
            Message = err;
        }
    }
}
