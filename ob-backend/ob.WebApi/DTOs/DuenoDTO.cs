using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class DuenoDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Apellido { get; set; }

        public DuenoDTO(Dueno dueno)
        {
            this.Nombre = dueno.Nombre;
            this.Email = dueno.Email;
            this.Apellido = dueno.Apellido;
        }
    }
}
