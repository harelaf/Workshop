using System.ComponentModel.DataAnnotations;

namespace MarketWeb.Server.DataLayer.DAL_Obects
{
    public class ComplaintItemDAL
    {
        [Key]
        public int complaintID { get; set; }
        public ComplaintDAL complaint { get; set; }

        public ComplaintItemDAL(int complaintID, ComplaintDAL complaint)
        {
            this.complaintID = complaintID;
            this.complaint = complaint;
        }

        public ComplaintItemDAL()
        {
            // ???
        }
    }
}
