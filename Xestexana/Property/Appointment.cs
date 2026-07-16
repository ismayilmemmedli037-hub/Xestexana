namespace Hospital.Properties
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public DateTime Tarix { get; set; }
        public string SaatAraligi { get; set; } = string.Empty;
        public bool IsReserved { get; set; } = false;
        public int? UserId { get; set; }
        public string? UserTamAd { get; set; }
        public DateTime? ReservedAt { get; set; }
    }
}