using Hospital.Message;

namespace Hospital.Back
{
    public static class AppServices
    {
        public static readonly AppointmentService AppointmentService = new();
        public static readonly DoctorService DoctorService = new(AppointmentService);
        public static readonly UserService UserService = new();
        public static readonly EmailService EmailService = new();
    }
}