using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class ConstructoraDTO
    {
        public string Nombre { get; set; }
        public ConstructoraDTO() { }    
        public ConstructoraDTO(Constructora constructora)
        {
            this.Nombre = constructora.Nombre;
        }

        public Constructora ToEntity()
        {
            return new Constructora(this.Nombre);
        }
    }
}
