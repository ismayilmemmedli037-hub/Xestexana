using Hospital.Properties;

namespace Hospital.Back
{
    public class DoctorService
    {
        private readonly string _filePath = Path.Combine("Data", "Doctors.json");
        private List<Doctor> _doctors;
        private readonly AppointmentService _appointmentService;

        public DoctorService(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
            _doctors = JsonFileService.Load<Doctor>(_filePath);

            if (_doctors.Count == 0)
            {
                SeedInitialDoctors();
            }
        }

        private void Save() => JsonFileService.Save(_filePath, _doctors);

        private void SeedInitialDoctors()
        {
            var seedList = new List<Doctor>
            {
                new Doctor { Ad = "Xeyyam", Soyad = "Qasimov", IsTecrubesi = 7, Sobe = Department.Pediatriya, Email = "xeyyam.memmedova@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                new Doctor { Ad = "Amil", Soyad = "Eyvazli", IsTecrubesi = 5, Sobe = Department.Pediatriya, Email = "amil.eyvazli@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                new Doctor { Ad = "Cavid", Soyad = "Ibadzade", IsTecrubesi = 10, Sobe = Department.Pediatriya, Email = "cavid.ibadzade@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                
                new Doctor { Ad = "Fatima", Soyad = "Abdullasoy", IsTecrubesi = 12, Sobe = Department.Travmatologiya, Email = "fatima.abdullasoy@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                new Doctor { Ad = "Isamyil", Soyad = "Memmedli", IsTecrubesi = 8, Sobe = Department.Travmatologiya, Email = "ismayilmemmedi037@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                
                new Doctor { Ad = "Ilham", Soyad = "Abbasova", IsTecrubesi = 6, Sobe = Department.Stomatologiya, Email = "gunel.abbasova@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                new Doctor { Ad = "Nicat", Soyad = "Rzayev", IsTecrubesi = 9, Sobe = Department.Stomatologiya, Email = "tural.rzayev@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                new Doctor { Ad = "Zehra", Soyad = "Şıxalizade", IsTecrubesi = 4, Sobe = Department.Stomatologiya, Email = "aytac.seferova@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
                new Doctor { Ad = "Yusuf", Soyad = "Ceferov", IsTecrubesi = 11, Sobe = Department.Stomatologiya, Email = "orxan.ceferov@gmail.com", Password = "1234", Status = DoctorStatus.Approved },
            };

            int id = 1;
            foreach (var d in seedList)
            {
                d.Id = id++;
                _doctors.Add(d);
            }
            Save();
            foreach (var d in _doctors)
            {
                _appointmentService.GenerateSlotsForDoctor(d.Id);
            }
        }

        public bool EmailExists(string email)
        {
            return _doctors.Any(d => d.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public Doctor ApplyForJob(string ad, string soyad, int tecrube, Department sobe, string email, string password)
        {
            var doctor = new Doctor
            {
                Id = _doctors.Count == 0 ? 1 : _doctors.Max(d => d.Id) + 1,
                Ad = ad,
                Soyad = soyad,
                IsTecrubesi = tecrube,
                Sobe = sobe,
                Email = email,
                Password = password,
                Status = DoctorStatus.Pending
            };

            _doctors.Add(doctor);
            Save();
            return doctor;
        }

        public Doctor? Login(string email, string password)
        {
            return _doctors.FirstOrDefault(d => d.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && d.Password == password);
        }

        public List<Doctor> GetPending() => _doctors.Where(d => d.Status == DoctorStatus.Pending).ToList();
        public List<Doctor> GetApprovedByDepartment(Department sobe) => _doctors.Where(d => d.Sobe == sobe && d.Status == DoctorStatus.Approved).ToList();
        public List<Doctor> GetAll() => _doctors;
        public Doctor? GetById(int id) => _doctors.FirstOrDefault(d => d.Id == id);

        public void Approve(int doctorId)
        {
            var doctor = GetById(doctorId);
            if (doctor == null)
            {
                return;
            }

            doctor.Status = DoctorStatus.Approved;
            Save();

            _appointmentService.GenerateSlotsForDoctor(doctor.Id);
        }

        public void Reject(int doctorId)
        {
            var doctor = GetById(doctorId);
            if (doctor == null)
            {
                return;
            }

            _doctors.Remove(doctor);
            Save();
        }
    }
}