namespace ob.Domain
{
    public class Categoria
    {

        private string _nombre;
        public string Nombre
        {
            get { return _nombre; }
            set { Validator.ValidateString(value); _nombre = value; }
        }

        public Categoria(string nombre)
        {
            _nombre = nombre;
        }
    }

}