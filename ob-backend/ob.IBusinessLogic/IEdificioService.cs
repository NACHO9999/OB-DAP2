using ob.Domain;
namespace ob.IBusinessLogic
{
    public interface IEdificioService
    {
        void CrearEdificio(Edificio edificio);
        void EditarEdificio(Edificio edificio);
        void BorrarEdificio(Edificio edificio);
        Edificio GetEdificioByNombre(string nombre);
        Edificio GetEdificioByNombreYDireccion(string nombre, string direccion);
    }
}