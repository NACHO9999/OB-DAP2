namespace ob.Domain
{
    public class Edificio
    {
        public string Nombre { get; set; }
        public string Dirección { get; set; }
        public string Ubicación { get; set; }
        public Constructora EmpresaConstructora { get; set; }
        public decimal GastosComunes { get; set; }
        public List<Depto> Deptos { get; set; }

        public Edificio(string nombre, string dirección, string ubicación, Constructora empresaConstructora, decimal gastosComunes, List<Depto> deptos)
        {
            Nombre = nombre;
            Dirección = dirección;
            Ubicación = ubicación;
            EmpresaConstructora = empresaConstructora;
            GastosComunes = gastosComunes;
            Deptos = deptos;
        }
    }
}