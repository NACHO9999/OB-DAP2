using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;
using System.Net;
using ob.Reflection.ImportData;

namespace ob.BusinessLogic
{


    public class AdminConstructoraService : IAdminConstructoraService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IEdificioService _edificioService;
        private readonly IConstructoraService _constructoraService;
        private readonly IEncargadoService _encargadoService;
        private readonly IDeptoService _deptoService;
        public AdminConstructoraService(IUsuarioRepository repository, IEdificioService edificioService, IConstructoraService constructoraService, IEncargadoService encargadoService, IDeptoService deptoService)
        {
            _repository = repository;
            _edificioService = edificioService;
            _constructoraService = constructoraService;
            _encargadoService = encargadoService;
            _deptoService = deptoService;
        }

        public void ImportarEdificios(List<EdificioData> edificioData, string email)
        {
            var admin = GetAdminConstructoraByEmail(email);
            if (admin.Constructora == null) {
                throw new InvalidOperationException("El admin no puede importar edificios");
            }
            foreach (var edificio in edificioData)
            {
                var deptos = new List<Depto>();
                foreach (var dep in edificio.Departamentos)
                {
                    var dueno = new Dueno("Desconocido", "Desconocido", dep.PropietarioEmail);
                    var depto = new Depto(dep.Piso, dep.numero_puerta, dueno, dep.Habitaciones,dep.Baños, dep.ConTerraza,edificio.Nombre, edificio.Direccion.calle_principal + " " + edificio.Direccion.numero_puerta + ", esq " + edificio.Direccion.calle_secundaria);

                }
                var edificioImportado = new Edificio(
                    edificio.Nombre,
                    edificio.Direccion.calle_principal + " " + edificio.Direccion.numero_puerta + ", esq " + edificio.Direccion.calle_secundaria,
                    edificio.Gps.Latitud + ", " + edificio.Gps.Longitud,
                    admin.Constructora,
                    edificio.gastos_comunes,
                    deptos

                    );
                _edificioService.CrearEdificio(edificioImportado);
                if (edificio.Encargado!= null){
                    try
                    {
                        AsignarEncargado(email, edificio.Encargado, edificio.Nombre, edificio.Direccion.calle_principal + " " + edificio.Direccion.numero_puerta + ", esq " + edificio.Direccion.calle_secundaria);
                    }
                    catch (Exception)
                    {
                    }
                }
                
            }
        }
        public void CrearAdminConstructora(AdminConstructora adminConstructora)
        {
            if (_repository.Get(u => u.Email.ToLower() == adminConstructora.Email.ToLower()) != null)
            {
                throw new AlreadyExistsException("El administrador de constructora ya existe.");
            }
            if (adminConstructora.Constructora != null)
            {
                if (_constructoraService.GetConstructoraByNombre(adminConstructora.Constructora.Nombre) == null)
                {
                    throw new KeyNotFoundException("No se encontró la constructora.");
                }
            }
            _repository.Insert(adminConstructora);
            _repository.Save();
        }
        public Constructora GetConstructora(string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            return usuario.Constructora;
        }
        public AdminConstructora GetAdminConstructoraByEmail(string email)
        {
            var usuario = _repository.Get(u => u.Email.ToLower() == email.ToLower(), new List<string> { "Constructora" });
            if (usuario is AdminConstructora adminConstructora)
            {
                return adminConstructora;
            }
            throw new KeyNotFoundException("El administrador de constructora no existe.");
        }
        public void CrearConstructora(string nombre, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora != null)
            {
                throw new AlreadyExistsException("El usuario ya tiene constructora.");
            }

            _constructoraService.CrearConstructora(nombre);
            usuario.Constructora = _constructoraService.GetConstructoraByNombre(nombre);
            _repository.Update(usuario);
            _repository.Save();
        }
        public void ElegirConstructora(string email, string nombre)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var constructora = _constructoraService.GetConstructoraByNombre(nombre);
            if (constructora == null)
            {
                throw new KeyNotFoundException("No se encontró la constructora.");
            }
            usuario.Constructora = constructora;
            _repository.Update(usuario);
            _repository.Save();
        }
        public bool TieneConstructora(string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            return usuario.Constructora != null;
        }
        public void CrearEdificio(Edificio edificio, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            edificio.EmpresaConstructora = usuario.Constructora;
            _edificioService.CrearEdificio(edificio);
        }
        public void BorrarEdificio(string nombre, string direccion, string email)
        {
            Edificio edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
            AdminConstructora admin = GetAdminConstructoraByEmail(email);
            if (edificio.EmpresaConstructora != admin.Constructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            _edificioService.BorrarEdificio(edificio);

        }
        public void EditarEdificio(Edificio edificio, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            if (edificio.EmpresaConstructora.Nombre != usuario.Constructora.Nombre)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            _edificioService.EditarEdificio(edificio);
        }

        public Edificio GetEdificioByNombreYDireccion(string nombre, string direccion, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
            if (usuario.Constructora != edificio.EmpresaConstructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            return edificio;
        }

        public void CrearDepto(string email, Depto depto)
        {
            var admin = GetAdminConstructoraByEmail(email);

            var edificio = _edificioService.GetEdificioByNombreYDireccion(depto.EdificioNombre, depto.EdificioDireccion);

            if (admin.Constructora != edificio.EmpresaConstructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            _deptoService.CrearDepto(depto);
            edificio.Deptos.Add(depto);
            _edificioService.EditarEdificio(edificio);
            _repository.Save();
        }
        public void EditarDepto(string email, Depto depto)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(depto.EdificioNombre, depto.EdificioDireccion);
            if (edificio.EmpresaConstructora.Nombre != usuario.Constructora?.Nombre)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            _deptoService.EditarDepto(depto);
        }

        public void BorrarDepto(string email, int numero, string nombre, string direccion)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
            if (edificio.EmpresaConstructora.Nombre != usuario.Constructora.Nombre)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            var depto = _deptoService.GetDepto(numero, nombre, direccion);
            _deptoService.BorrarDepto(depto);
        }
        public Depto GetDepto(int numero, string nombre, string direccion, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
            if (edificio.EmpresaConstructora.Nombre != usuario.Constructora?.Nombre)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            return _deptoService.GetDepto(numero, nombre, direccion);
        }
        public void EditarConstructora(Constructora constructora, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            constructora.Id = usuario.Constructora.Id;
            _constructoraService.EditarConstructora(constructora);
        }
        public void AsignarConstructora(string email, string nombreConstructora)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var constructora = _constructoraService.GetConstructoraByNombre(nombreConstructora);
            if (constructora == null)
            {
                throw new KeyNotFoundException("No se encontró la constructora.");
            }
            if (usuario.Constructora != null)
            {
                throw new AlreadyExistsException("El usuario ya tiene constructora.");
            }
            usuario.Constructora = constructora;
            _repository.Update(usuario);
            _repository.Save();
        }
        public void DesasignarEncargado(string email, string edNombre, string edDireccion)
        {
            var admin = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(edNombre, edDireccion);
            if (edificio.EmpresaConstructora != admin.Constructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            var listaEncargados = _encargadoService.GetAllEncargados();
            foreach (var encargado in listaEncargados)
            {
                if (encargado.Edificios.Contains(edificio))
                {
                    encargado.Edificios.Remove(edificio);
                    _repository.Update(encargado);
                    _repository.Save();
                }
            }
        }
        public void AsignarEncargado(string email, string emailEncargado, string nombreEdificio, string direccionEdificio)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var encargado = _encargadoService.GetEncargadoByEmail(emailEncargado);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombreEdificio, direccionEdificio);
            if (edificio.EmpresaConstructora.Nombre != usuario.Constructora?.Nombre)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            var listaEncargados = _encargadoService.GetAllEncargados();
            foreach (var enc in listaEncargados)
            {
                if (enc.Edificios.Contains(edificio) && enc != encargado)
                {
                    enc.Edificios.Remove(edificio);
                }
            }
            if (!encargado.Edificios.Contains(edificio))
            {
                encargado.Edificios.Add(edificio);
                _repository.Update(encargado);
                _repository.Save();
            }
        }
        public List<Edificio> GetEdificiosPorAdmin(string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            return _edificioService.GetAllEdificios().Where(e => e.EmpresaConstructora == usuario.Constructora).ToList();
        }
        public List<Edificio> GetEdificiosSinEncargado(string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            var listaEdificios = GetEdificiosPorAdmin(email);
            var listaEncargados = _encargadoService.GetAllEncargados();
            foreach (var encargado in listaEncargados)
            {
                foreach (var edificio in encargado.Edificios)
                {
                    if (listaEdificios.Contains(edificio))
                    {
                        listaEdificios.Remove(edificio);
                    }
                }
            }
            return listaEdificios;
        }
        public List<Edificio> GetEdificiosConEncargado(string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }

            var listaEncargados = _encargadoService.GetAllEncargados();


            var listaEdificiosConEncargado = new List<Edificio>();

            foreach (var encargado in listaEncargados)
            {

                foreach (var edificio in encargado.Edificios)
                {
                    if (edificio.EmpresaConstructora == usuario.Constructora)
                    {
                        listaEdificiosConEncargado.Add(edificio);
                    }
                }
            }

            return listaEdificiosConEncargado;
        }

        public List<Edificio> FiltrarPorNombreDeEncargado(string  email, string nombreEncargado)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var listaEncargados = _encargadoService.GetAllEncargados();
            var encargados = listaEncargados.Where(e => e.Nombre.ToLower().Contains(nombreEncargado.ToLower()));
            var retorno = new List<Edificio>();

            foreach (var encargado in encargados)
            {
                foreach (var edificio in encargado.Edificios)
                {
                    if (edificio.EmpresaConstructora == usuario.Constructora)
                    {
                        retorno.Add(edificio);
                    }
                }
            }
            return retorno;
        }
        public List<Constructora> GetConstructoras()
        {
            return _constructoraService.GetAllConstructoras().ToList();
        }
    }
}
