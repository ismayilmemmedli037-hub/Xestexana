using Hospital.Properties;

namespace Hospital.Back
{
    public class UserService
    {
        private readonly string _filePath = Path.Combine("Data", "users.json");
        private List<User> _users;

        public UserService()
        {
            _users = JsonFileService.Load<User>(_filePath);
        }

        private void Save() => JsonFileService.Save(_filePath, _users);

        public bool EmailExists(string email)
        {
            return _users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User Register(string ad, string soyad, string email, string telefon, string password)
        {
            var user = new User
            {
                Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1,
                Ad = ad,
                Soyad = soyad,
                Email = email,
                Telefon = telefon,
                Password = password
            };

            _users.Add(user);
            Save();
            return user;
        }

        public User? Login(string email, string password)
        {
            return _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);
        }
        public List<User> GetAll() => _users;
    }
}