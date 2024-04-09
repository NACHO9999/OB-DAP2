namespace ob.Domain
{
    public class Depto
    {
        public int Piso { get; set; }
        public int Numero { get; set; }
        public Dueno Dueno { get; set; }
        public int CantidadCuartos { get; set; }
        public int CantidadBanos { get; set; }
        public bool ConTerraza { get; set; }
    }
}