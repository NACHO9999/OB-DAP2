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
            this.Nombre = encargado.Nombre;
            this.Email = encargado.Email;
            this.Contrasena = encargado.Contrasena;
            this.Edificios = encargado.Edificios.Select(e => new EdificioDTO(e)).ToList();
        }
    }
}
