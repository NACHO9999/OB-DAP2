namespace ob.Domain
{
    public class Constructora
    {
        public Guid Id { get; set; }
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


        public Constructora(string nombre)
        {
            Id = Guid.NewGuid();
            _nombre = nombre;
        }
    }
}