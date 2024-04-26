namespace ob.Domain
{
    public class Edificio
    {
        private string _nombre;
        public string Nombre {
            get { return _nombre; }
            set {
                Validator.ValidateStringMaxLength(value, 100);
                Validator.ValidateString(value);
                _nombre = value;
            }
        }
        private string _dirección;
        public string Direccion {
            get { return _dirección; }
            set {
                Validator.ValidateStringMaxLength(value, 200);
                Validator.ValidateString(value);
                _dirección = value;
            }
        }
        private string _ubicación;
        public string Ubicación {
            get { return _ubicación; }
            set {
                Validator.ValidateStringMaxLength(value, 100);
                Validator.ValidateString(value);
                _ubicación = value;
            }
         }
         private Constructora _empresaConstructora;
        public Constructora EmpresaConstructora { 
            get { return _empresaConstructora; }
            set {
                Validator.IsNotNull(value);
                _empresaConstructora = value;
            }
         }
        private decimal _gastosComunes;
        public decimal GastosComunes { 
            get { return _gastosComunes; }
            set {
                Validator.ValidateDecimal(value,0,100000000);
                _gastosComunes = value;
            }
         }
         private List<Depto> _deptos;
        public List<Depto> Deptos { 
            get { return _deptos; }
            set {
                Validator.IsNotNull(value);
                _deptos = value;
            }
        }

        public Edificio(string nombre, string dirección, string ubicación, Constructora empresaConstructora, decimal gastosComunes, List<Depto> deptos)
        {
            _nombre = nombre;
            _dirección = dirección;
            _ubicación = ubicación;
            _empresaConstructora = empresaConstructora;
            _gastosComunes = gastosComunes;
            _deptos = deptos;
        }
    }
}