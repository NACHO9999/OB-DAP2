using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class InvitacionDTO
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaExpiracion { get; set; }
        
        public InvitacionDTO() { }

        public InvitacionDTO(Invitacion invitacion)
        {
            this.Email = invitacion.Email;
            this.Nombre = invitacion.Nombre;
            this.FechaExpiracion = invitacion.FechaExpiracion;
        }
    }
    
}
