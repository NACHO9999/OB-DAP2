using ob.Domain;

namespace ob.IBusinessLogic;

public interface ICategoriaService
{
    void CrearCategoria(Categoria categoria);
    Categoria GetCategoriaByNombre(string nombre);
}