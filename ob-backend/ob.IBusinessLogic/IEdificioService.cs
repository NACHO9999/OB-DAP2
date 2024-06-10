using ob.Domain;
namespace ob.IBusinessLogic
{
    public interface IEdificioService
    {
        public List<Edificio> GetAllEdificios();
        void CrearEdificio(Edificio edificio);
        void CrearEdificioConDatos(string nombre, string direccion, string ubicacion, string constructora, decimal gastos, List<Depto> deptos);
        void EditarEdificio(Edificio edificio);
        void BorrarEdificio(Edificio edificio);
        Edificio GetEdificioByNombreYDireccion(string nombre, string direccion);
        bool EdificioExists(string nombre, string direccion);
        void AgregarDepto(Edificio edificio, Depto depto);
    }
}