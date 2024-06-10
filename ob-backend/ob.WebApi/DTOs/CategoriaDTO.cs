using ob.Domain;
namespace ob.WebApi.DTOs
{
    public class CategoriaDTO
    {
        public string Nombre { get; set; }

        public CategoriaDTO() { } 

        public CategoriaDTO(Categoria categoria)
        {
            this.Nombre = categoria.Nombre;
        }
    }
}