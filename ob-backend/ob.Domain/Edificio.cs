namespace ob.Domain
{
    public class Edificio
    {
        public string Nombre { get; set; }
        public string Dirección { get; set; }
        public string Ubicación { get; set; }
        public string EmpresaConstructora { get; set; }
        public decimal GastosComunes { get; set; }
        public List<Depto> Deptos { get; set; }
    }
}