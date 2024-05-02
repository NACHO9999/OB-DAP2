namespace ob.Domain
{
    public class Invitacion
    {
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
        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set
            {
                Validator.ValidateStringMaxLength(value, 100);
                Validator.ValidateString(value);
                _nombre = value;
            }
        }
        private DateTime _fechaExpiracion;
        public DateTime FechaExpiracion
        {
            get { return _fechaExpiracion; }
            set
            {
                Validator.ValidateFutureDate(value);
                _fechaExpiracion = value;
            }

        }
        public Invitacion(string email, string nombre, DateTime fechaExpiracion)
        {
            _email = email;
            _nombre = nombre;
            _fechaExpiracion = fechaExpiracion;
        }

    }
}