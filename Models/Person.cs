namespace Hospital_Test_Performance.Models
{
    public class Person
    {
        // Campos privados
        private int _id;
        private string _name = string.Empty;
        private DateTime _dateOfBirth;
        private string _phone = string.Empty;
        private string _address = string.Empty;
        private string _email = string.Empty;
        // Propiedades públicas que exponen los campos
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _name = value;
            }
        }

        public DateTime DateOfBirth
        {
            get { return _dateOfBirth; }
            set { _dateOfBirth = value; }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - _dateOfBirth.Year;
                if (_dateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
        public string Telefono
        {
            get { return _phone; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _phone = value;
            }
        }
        public string Address
        {
            get { return _address; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _address = value;
            }
        }
        public string Email
        {
            get { return _email; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    _email = value;
            }
        }
    }
}