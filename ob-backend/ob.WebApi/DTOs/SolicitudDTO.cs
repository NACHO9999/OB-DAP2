using Enums;
using ob.Domain;
using System.Text.Json.Serialization;

namespace ob.WebApi.DTOs
{
    public class SolicitudDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("perMan")]
        public UsuarioCreateModel? PerMan { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("depto")]
        public DeptoDTO Depto { get; set; }

        [JsonPropertyName("categoria")]
        public CategoriaDTO Categoria { get; set; }

        [JsonPropertyName("estado")]
        public EstadoSolicitud Estado { get; set; }

        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonPropertyName("fechaFin")]
        public DateTime? FechaFin { get; set; }

        [JsonConstructor]
        public SolicitudDTO(Guid id, UsuarioCreateModel? perMan, string descripcion, DeptoDTO depto, CategoriaDTO categoria, EstadoSolicitud estado, DateTime fechaInicio, DateTime? fechaFin)
        {
            Id = id;
            PerMan = perMan;
            Descripcion = descripcion;
            Depto = depto;
            Categoria = categoria;
            Estado = estado;
            FechaInicio = fechaInicio;
            FechaFin = fechaFin;
        }

        public SolicitudDTO(Solicitud solicitud)
        {
            Id = solicitud.Id;
            PerMan = solicitud.PerMan != null ? new UsuarioCreateModel
            {
                Nombre = solicitud.PerMan.Nombre,
                Apellido = solicitud.PerMan.Apellido,
                Email = solicitud.PerMan.Email,
                Contrasena = solicitud.PerMan.Contrasena
            } : null;
            Descripcion = solicitud.Descripcion;
            Depto = new DeptoDTO(solicitud.Depto);
            Categoria = new CategoriaDTO(solicitud.Categoria);
            Estado = solicitud.Estado;
            FechaInicio = solicitud.FechaInicio;
            FechaFin = solicitud.FechaFin;
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
            var solicitud = new Solicitud(MantenimientoNullCheck(PerMan), Descripcion, Depto.ToEntity(), new Categoria(Categoria.Nombre), Estado, FechaInicio)
            {
                FechaFin = FechaFin
            };
            solicitud.Id = Id;
            return solicitud;
        }
    }
}
