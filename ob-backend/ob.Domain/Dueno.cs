namespace ob.Domain
{
    public class Dueno
    {

        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                Validator.ValidateString(value);
                Validator.ValidateStringMaxLength(value, 100);
                _nombre = value;
            }
        }
        private string _apellido;
        public string Apellido
        {
            get { return _apellido; }
            set
            {
                Validator.ValidateString(value);
                Validator.ValidateStringMaxLength(value, 100);
                _apellido = value;
            }
        }
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                Validator.ValidateEmail(value);
                Validator.ValidateStringMaxLength(value, 320);
                _email = value;
            }
        }


        public Dueno(string nombre, string apellido, string email)
        {
            _nombre = nombre;
            _apellido = apellido;
            _email = email;
        }
    }
}