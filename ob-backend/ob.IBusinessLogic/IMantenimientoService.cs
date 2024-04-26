using ob.Domain;
namespace ob.IBusinessLogic
{
    public interface IMantenimientoService
    {
        void CrearMantenimiento(Mantenimiento mantenimiento);
        Mantenimiento GetMantenimientoByEmail(string email);
        void AtenderSolicitud(Solicitud solicitud); 
        void CompletarSolicitud(Solicitud solicitud);
    }
    
}