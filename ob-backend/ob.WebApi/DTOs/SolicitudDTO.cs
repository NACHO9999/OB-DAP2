using Enums;
using ob.Domain;
namespace ob.WebApi.DTOs
{
    public class SolicitudDTO
    {
        public Guid Id { get; set; }
        public UsuarioCreateModel Mantenimento { get; set; }
        public string Descripcion { get; set; }
        public DeptoDTO Depto { get; set; }
        public CategoriaDTO Categoria { get; set; }
        public EstadoSolicitud Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public SolicitudDTO(Solicitud solicitud)
        {
            this.Id = solicitud.Id;
            this.Mantenimento = new UsuarioCreateModel() { Nombre = solicitud.PerMan.Nombre, Email = solicitud.PerMan.Email, Apellido = solicitud.PerMan.Apellido, Contrasena=solicitud.PerMan.Contrasena };
            this.Descripcion = solicitud.Descripcion;
            this.Depto = new DeptoDTO(solicitud.Depto);
            this.Categoria = new CategoriaDTO(solicitud.Categoria);
            this.Estado = solicitud.Estado;
            this.FechaInicio = solicitud.FechaInicio;
            this.FechaFin = solicitud.FechaFin;
        }
        public Solicitud ToEntity()
        {
            return new Solicitud(this.Mantenimento.MantenimientoToEntity(), this.Descripcion, this.Depto.ToEntity(), new Categoria(this.Categoria.Nombre), this.Estado, this.FechaInicio) { FechaFin = this.FechaFin};
        }
    }
}
