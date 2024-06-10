using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class EdificioCreateDTO
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Ubicacion { get; set; }
        public decimal GastosComunes { get; set; }
        public List<DeptoDTO> Deptos { get; set; }

        public EdificioCreateDTO()
        {
        }
        

        
    }
}
