using ob.Domain;
namespace ob.IBusinessLogic;
public interface IConstructoraService
{
    void CrearConstructora(Constructora constructora);
    Constructora GetConstructoraByNombre(string nombre);
    IEnumerable<Constructora> GetAllConstructoras();
    public void EditarConstructora(Constructora constructora);



}