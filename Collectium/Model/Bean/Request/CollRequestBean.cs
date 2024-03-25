using System.ComponentModel.DataAnnotations.Schema;

namespace Collectium.Model.Bean.Request
{
    public class ReassignBean
    {
        public IList<int>? Loan { get; set; }

        public int? ToMember { get; set; }
    }

    public class ReassignWrapper
    {
        public IList<int>? Loan { get; set; }

        public int? ToMember { get; set; }

        public string? ToRole { get; set; }
    }

    public class SaveResultBean
    {
        public int? LoanId { get; set; }

        public int? ReasonId { get; set; }

        public int? ResultId { get; set; }

        public int? AddId { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public double? Amount { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? ResultDate { get; set; }

        public string? ResultTime { get; set; }


    }

    public class SaveResultBeanToDc
    {
        public int? LoanId { get; set; }

        public int? ReasonId { get; set; }

        public int? ResultId { get; set; }

        public int? AddId { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public double? Amount { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? ResultDate { get; set; }

        public string? ResultTime { get; set; }

        public int? ToFcId { get; set; }
        public int? DPD { get; set; }

    }

    public class SaveResultBeanFc
    {
        public int? LoanId { get; set; }

        public int? ReasonId { get; set; }

        public int? ResultId { get; set; }

        public int? AddId { get; set; }

        public string? Name { get; set; }

        public string? Notes { get; set; }

        public double? Amount { get; set; }

        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public string? ResultDate { get; set; }

        public string? ResultTime { get; set; }

        public List<int>? PhotoId { get; set; }


    }

    public class SaveContactBean
    {
        public int? LoanId { get; set; }

        public string? AddPhone { get; set; }

        public string? AddAddress { get; set; }

        public string? AddCity { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public List<int>? PhotoId { get; set; }
    }

    public class SaveContactDTOBean
    {
        public int? LoanId { get; set; }

        public string? AddPhone { get; set; }

        public string? AddAddress { get; set; }

        public string? AddCity { get; set; }

        public string? AddFrom { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }

        public List<int>? PhotoId { get; set; }
    }

    public class SaveContactResponseBean
    {
        public int? Id { get; set; }

        public int? LoanId { get; set; }

        public string? AddPhone { get; set; }

        public string? AddAddress { get; set; }

        public string? AddCity { get; set; }

        public string? AddFrom { get; set; }
    }

    public class TrackingBean
    {

        public double? Lat { get; set; }

        public double? Lon { get; set; }
    }
}
