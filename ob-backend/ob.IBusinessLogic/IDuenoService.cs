using ob.Domain;
namespace ob.IBusinessLogic;
public interface IDuenoService
{
    void CrearDueno(Dueno dueno);
    void BorrarDueno(Dueno dueno);
    Dueno GetDuenoByEmail(string email);
    bool DuenoExists(string email);
}