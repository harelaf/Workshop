using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer
{
    public class StringData
    {
        [Key]
        public int id { get; set; }
        [Required]
        public string data { get; set; }
        public StringData(string str)
        {
            data = str;
        }
        public StringData()
        {
            data = "";
        }
    }
}
