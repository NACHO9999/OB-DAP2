using ob.Domain;
namespace ob.IBusinessLogic;
public interface IConstructoraService
{
    void CrearConstructora(Constructora constructora);
    Constructora GetConstructoraByNombre(string nombre);

}