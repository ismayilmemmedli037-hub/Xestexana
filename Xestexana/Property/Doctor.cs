namespace Hospital.Properties
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public int IsTecrubesi { get; set; }
        public Department Sobe { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DoctorStatus Status { get; set; } = DoctorStatus.Pending;
        public string TamAd => $"Dr. {Ad} {Soyad}";
    }
}