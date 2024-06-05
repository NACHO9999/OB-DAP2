using ob.Domain;
namespace ob.IBusinessLogic;


public interface ISessionService
{
    Usuario? GetCurrentUser(Guid? authToken = null);
    Guid Authenticate(string email, string password);
    void Logout(Guid authToken);
    public string GetUserRole(string email, string password);
}