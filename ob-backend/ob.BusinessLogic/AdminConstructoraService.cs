using ob.Domain;
using ob.IBusinessLogic;
using ob.IDataAccess;
using ob.Exceptions.BusinessLogicExceptions;
using System.Net;

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
        public void CrearAdminConstructora(AdminConstructora adminConstructora)
        {
            if (_repository.Get(u => u.Email.ToLower() == adminConstructora.Email.ToLower()) != null)
            {
                throw new AlreadyExistsException("El administrador de constructora ya existe.");
            }
            _repository.Insert(adminConstructora);
            _repository.Save();
        }
        public AdminConstructora GetAdminConstructoraByEmail(string email)
        {
            var usuario = _repository.Get(u => u.Email.ToLower() == email.ToLower());
            if (usuario is AdminConstructora adminConstructora)
            {
                return adminConstructora;
            }
            throw new KeyNotFoundException("El administrador de constructora no existe.");
        }   
        public void CrearConstructora(Constructora constructora, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora!=null)
            {
                throw new AlreadyExistsException("El usuario ya tiene constructora.");
            }
            _constructoraService.CrearConstructora(constructora);
            _repository.Save();
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
            if(edificio.EmpresaConstructora != admin.Constructora)
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
            if (edificio.EmpresaConstructora != usuario.Constructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            _edificioService.EditarEdificio(edificio);
        }

        public Edificio GetEdificioByNombreYDireccion(string nombre, string direccion, string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
            if(usuario.Constructora != edificio.EmpresaConstructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            return edificio;
        }

        public void CrearDepto(string email, Depto depto)
        {
            var admin = GetAdminConstructoraByEmail(email);

            var edificio = _edificioService.GetEdificioByNombreYDireccion(depto.EdificioNombre, depto.EdificioDireccion);
            if (admin.Constructora!=edificio.EmpresaConstructora)
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
            if (edificio.EmpresaConstructora != usuario.Constructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            _deptoService.EditarDepto(depto);
        }

        public void BorrarDepto(string email, int numero, string nombre, string direccion)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombre, direccion);
            if (edificio.EmpresaConstructora != usuario.Constructora)
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
            if (edificio.EmpresaConstructora != usuario.Constructora)
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
            if(usuario.Constructora != null)
            {
                throw new AlreadyExistsException("El usuario ya tiene constructora.");
            }
            usuario.Constructora = constructora;
            _repository.Update(usuario);
            _repository.Save();
        }

        public void AsignarEncargado(string email, string emailEncargado, string nombreEdificio, string direccionEdificio)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            var encargado = _encargadoService.GetEncargadoByEmail(emailEncargado);
            var edificio = _edificioService.GetEdificioByNombreYDireccion(nombreEdificio, direccionEdificio);
            if (edificio.EmpresaConstructora != usuario.Constructora)
            {
                throw new InvalidOperationException("El edificio no pertenece a la constructora del usuario.");
            }
            var listaEncargados = _encargadoService.GetAllEncargados();
            foreach (var enc in listaEncargados)
            {
                if (enc.Edificios.Contains(edificio)&&enc!=encargado)
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
            return _edificioService.GetAllEdificios().Where(e => e.EmpresaConstructora==usuario.Constructora).ToList();
        }
        public List<Edificio> ListarEdificiosSinEncargado(string email)
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
        public List<Edificio> ListarEdificiosConEncargado(string email)
        {
            var usuario = GetAdminConstructoraByEmail(email);
            if (usuario.Constructora == null)
            {
                throw new KeyNotFoundException("El usuario no tiene constructora.");
            }
            var listaEdificios = GetEdificiosPorAdmin(email);
            var listaEncargados = _encargadoService.GetAllEncargados();
            var listaEdificiosConEncargado = new List<Edificio>();
            foreach (var encargado in listaEncargados)
            {
                foreach (var edificio in encargado.Edificios)
                {
                    if (listaEdificios.Contains(edificio))
                    {
                        listaEdificiosConEncargado.Add(edificio);
                    }
                }
            }
            return listaEdificiosConEncargado;
        }
        public List<Edificio> FiltrarPorNmobreDeEdificio(List<Edificio> edificios, string nombre)
        {
            return edificios.Where(e => e.Nombre == nombre).ToList();
        }
        public List<Edificio> FiltrarPorNombreDeEncargado(List<Edificio> edificios, string nombreEncargado)
        {
            var listaEncargados = _encargadoService.GetAllEncargados();
            var encargados = listaEncargados.Where(e => e.Nombre == nombreEncargado);
            var retorno = new List<Edificio>();

            if (encargados.Count()==0)
            {
                throw new KeyNotFoundException("No se encontró el encargado.");
            }
            foreach (var encargado in encargados)
            {
                foreach (var edificio in edificios)
                {
                    if (encargado.Edificios.Contains(edificio))
                    {
                        retorno.Add(edificio);
                    }
                }
            }
            return retorno;
        }
    }
}
