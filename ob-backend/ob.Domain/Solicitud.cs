using Enums;
namespace ob.Domain

{
    public class Solicitud
    {
        public Mantenimiento PerMan { get; set; }
        public string Descripcion { get; set; }
        public Depto Depto { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public Solicitud(Mantenimiento perMan, string descripcion, Depto depto, EstadoSolicitud estado, DateTime fechaInicio, DateTime fechaFin)
        {
            PerMan = perMan;
            Descripcion = descripcion;
            Depto = depto;
            Estado = estado;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        public Solicitud() { }
    }
}