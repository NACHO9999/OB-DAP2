using Enums;
using ob.Domain;
namespace ob.WebApi.DTOs
{
    public class SolicitudDTO
    {
        public Guid Id { get; set; }
        public UsuarioCreateModel? PerMan { get; set; }
        public string Descripcion { get; set; }
        public DeptoDTO Depto { get; set; }
        public CategoriaDTO Categoria { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public SolicitudDTO() { }
        public SolicitudDTO(Solicitud solicitud)
        {
            this.Id = solicitud.Id;
            if (solicitud.PerMan != null)
            {
                this.PerMan = new UsuarioCreateModel { Nombre = solicitud.PerMan.Nombre, Apellido = solicitud.PerMan.Apellido, Email = solicitud.PerMan.Email, Contrasena = solicitud.PerMan.Contrasena };
            }
            else
            {
                this.PerMan = null;
            }
            this.Descripcion = solicitud.Descripcion;
            this.Depto = new DeptoDTO(solicitud.Depto);
            this.Categoria = new CategoriaDTO(solicitud.Categoria);
            this.Estado = solicitud.Estado;
            this.FechaInicio = solicitud.FechaInicio;
            this.FechaFin = solicitud.FechaFin;
        }
        private Mantenimiento? MantenimientoNullCheck(UsuarioCreateModel? mantenimiento)
        {
            if (mantenimiento == null)
            {
                return null;
            }
            return new Mantenimiento(mantenimiento.Nombre, mantenimiento.Apellido, mantenimiento.Email, mantenimiento.Contrasena);
        }
        public Solicitud ToEntity()
        {
            var solicitud = new Solicitud(MantenimientoNullCheck(this.PerMan), this.Descripcion, this.Depto.ToEntity(), new Categoria(this.Categoria.Nombre), this.Estado, this.FechaInicio) { FechaFin = this.FechaFin };
            solicitud.Id = this.Id;
            return solicitud;
        }
    }
}
