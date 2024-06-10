

using Microsoft.Extensions.DependencyInjection;
using ob.BusinessLogic;
using ob.DataAccess;
using ob.IDataAccess;
using ob.IBusinessLogic;
using ob.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Cors;
using AppContext = ob.DataAccess.AppContext;
using System.Runtime.Serialization;

namespace ob.ServicesFactory
{
    public class ServicesFactory
    {

        public ServicesFactory() { }


        public void RegistrateServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<DbContext, AppContext>();
            serviceCollection.AddScoped<IUsuarioRepository, UsuarioRepository>();
            serviceCollection.AddScoped<IGenericRepository<Usuario>, UsuarioRepository>();
            serviceCollection.AddScoped<IGenericRepository<Constructora>, ConstructoraRepository>();
            serviceCollection.AddScoped<IGenericRepository<Session>, SessionRepository>();
            serviceCollection.AddScoped<IGenericRepository<Categoria>, CategoriaRepository>();
            serviceCollection.AddScoped<IGenericRepository<Dueno>, DuenoRepository>();
            serviceCollection.AddScoped<IGenericRepository<Edificio>, EdificioRepository>();
            serviceCollection.AddScoped<IGenericRepository<Depto>, DeptoRepository>();
            serviceCollection.AddScoped<IGenericRepository<Invitacion>, InvitacionRepository>();
            serviceCollection.AddScoped<IGenericRepository<Solicitud>, SolicitudRepository>();

            serviceCollection.AddScoped<IImporterLogic, ImporterLogic>();
            serviceCollection.AddScoped<IInvitacionService, InvitacionService>();
            serviceCollection.AddScoped<ISolicitudService, SolicitudService>();
            serviceCollection.AddScoped<IDuenoService, DuenoService>();
            serviceCollection.AddScoped<IDeptoService, DeptoService>();
            serviceCollection.AddScoped<ICategoriaService, CategoriaService>();
            serviceCollection.AddScoped<IConstructoraService, ConstructoraService>();
            serviceCollection.AddScoped<IEdificioService, EdificioService>();
            serviceCollection.AddScoped<ISessionService, SessionService>();
            serviceCollection.AddScoped<IAdminService, AdminService>();
            serviceCollection.AddScoped<IEncargadoService, EncargadoService>();
            serviceCollection.AddScoped<IMantenimientoService, MantenimientoService>();
            serviceCollection.AddScoped<IAdminConstructoraService, AdminConstructoraService>();




            serviceCollection.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins("http://localhost:8080", "http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

        }

    }
}
