using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class ConstructoraDTO
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public ConstructoraDTO() { }    
        public ConstructoraDTO(Constructora constructora)
        {
            this.Id = constructora.Id;
            this.Nombre = constructora.Nombre;
        }

        public Constructora ToEntity()
        {
            var constructora = new Constructora(this.Nombre);
            constructora.Id = this.Id;
            return constructora;
        }
    }
}
