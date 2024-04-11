namespace ob.Domain
{
    public class Depto
    {
        private int _piso;
        public int Piso
        {
            get { return _piso; }
            set { Validator.ValidateInt(value, 0, 136); _piso = value; }
        }
        private int _numero;
        public int Numero {
            get { return _numero; }
            set { Validator.ValidateInt(value, 0, 13700); _numero = value; }
        }
        public Dueno? Dueno { get; set; }
        private int _cantidadCuartos;
        public int CantidadCuartos {
            get { return _cantidadCuartos; }
            set { Validator.ValidateInt(value, 1, 40); _cantidadCuartos = value; }
        }
        private int _cantidadBanos;
        public int CantidadBanos {
            get { return _cantidadBanos; }
            set { Validator.ValidateInt(value, 1, 40); _cantidadBanos = value; }
        }
        private bool _conTerraza;
        public bool ConTerraza {
            get { return _conTerraza; }
            set { Validator.IsNotNull(value); _conTerraza = value; }   
        }

        public Depto(int piso, int numero, Dueno? dueno, int cantidadCuartos, int cantidadBanos, bool conTerraza)
        {
            _piso = piso;
            _numero = numero;
            Dueno = dueno;
            _cantidadCuartos = cantidadCuartos;
            _cantidadBanos = cantidadBanos;
            _conTerraza = conTerraza;
        }
    }
}