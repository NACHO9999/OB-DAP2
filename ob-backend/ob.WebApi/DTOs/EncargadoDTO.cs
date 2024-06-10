using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class EncargadoDTO
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public List<EdificioDTO> Edificios { get; set; }

        public EncargadoDTO() { }

        public EncargadoDTO(Encargado encargado)
        {
            if (encargado == null)
            {
                throw new ArgumentNullException(nameof(encargado), "The provided encargado is null.");
            }

            this.Nombre = encargado.Nombre ?? string.Empty;
            this.Email = encargado.Email ?? string.Empty;
            this.Contrasena = encargado.Contrasena ?? string.Empty;
            this.Edificios = encargado.Edificios?.Select(e => new EdificioDTO(e)).ToList() ?? new List<EdificioDTO>();

            // Log the state of the object
            Console.WriteLine($"Created EncargadoDTO: Nombre={this.Nombre}, Email={this.Email}");
        }
    }
}
