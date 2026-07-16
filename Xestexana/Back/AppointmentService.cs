using Hospital.Properties;

namespace Hospital.Back
{
    public class AppointmentService
    {
        private readonly string _filePath = Path.Combine("Data", "appointments.json");
        private List<Appointment> _appointments;

        public static readonly string[] SaatAraliqlari = { "09:00-11:00", "12:00-14:00", "15:00-17:00" };
        private const int GunSayi = 7;

        public AppointmentService()
        {
            _appointments = JsonFileService.Load<Appointment>(_filePath);
        }

        private void Save() => JsonFileService.Save(_filePath, _appointments);
        public void GenerateSlotsForDoctor(int doctorId)
        {
            bool changed = false;

            for (int i = 0; i < GunSayi; i++)
            {
                DateTime tarix = DateTime.Today.AddDays(i);

                foreach (var saat in SaatAraliqlari)
                {
                    bool exists = _appointments.Any(a =>
                        a.DoctorId == doctorId &&
                        a.Tarix.Date == tarix.Date &&
                        a.SaatAraligi == saat);

                    if (!exists)
                    {
                        _appointments.Add(new Appointment
                        {
                            Id = _appointments.Count == 0 ? 1 : _appointments.Max(a => a.Id) + 1,
                            DoctorId = doctorId,
                            Tarix = tarix,
                            SaatAraligi = saat,
                            IsReserved = false
                        });
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                Save();
            }
        }

        public List<DateTime> GetAvailableDatesForDoctor(int doctorId)
        {
            return _appointments
                .Where(a => a.DoctorId == doctorId)
                .Select(a => a.Tarix.Date)
                .Distinct()
                .OrderBy(d => d)
                .ToList();
        }

        public List<Appointment> GetSlotsForDoctorAndDate(int doctorId, DateTime tarix)
        {
            return _appointments
                .Where(a => a.DoctorId == doctorId && a.Tarix.Date == tarix.Date)
                .OrderBy(a => a.SaatAraligi)
                .ToList();
        }

        public Appointment? GetById(int id) => _appointments.FirstOrDefault(a => a.Id == id);

        public bool Reserve(int appointmentId, User user)
        {
            var slot = GetById(appointmentId);
            if (slot == null || slot.IsReserved)
            {
                return false;
            }

            slot.IsReserved = true;
            slot.UserId = user.Id;
            slot.UserTamAd = user.TamAd;
            slot.ReservedAt = DateTime.Now;
            Save();
            return true;
        }

        public List<Appointment> GetByDoctor(int doctorId) => _appointments.Where(a => a.DoctorId == doctorId && a.IsReserved).ToList();

        public List<Appointment> GetByUser(int userId) => _appointments.Where(a => a.UserId == userId).ToList();

        public List<Appointment> GetAllReserved() => _appointments.Where(a => a.IsReserved).ToList();
    }
}