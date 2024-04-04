using Enums;
namespace ob.Domain

{
    public class Solicitud
    {
        public PersonaMantenimiento PerMan { get; set; }
        public string Descripcion { get; set; }
        public Depto Depto { get; set; }
        public EstadoSolcitud Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}