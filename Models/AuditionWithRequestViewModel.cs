namespace _1.Models
{
    public class AuditionWithRequestViewModel
    {
        public int AuditionRequestId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime AuditionDate { get; set; }  // По умолчанию будет DateTime.MinValue
        public string ResponsibleEmployeeName { get; set; }
        public int? ResponsibleEmployeeId { get; set; }
        public bool IsAuditionAssigned => AuditionDate != DateTime.MinValue;

    }

}
