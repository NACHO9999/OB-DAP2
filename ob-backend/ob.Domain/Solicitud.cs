using Enums;
namespace ob.Domain

{
    public class Solicitud
    {
        public Guid Id { get; set; }
        public Mantenimiento? PerMan;

        private string _descripcion;
        public string Descripcion
        {
            get { return _descripcion; }
            set
            {
                Validator.ValidateString(value);
                Validator.ValidateStringMaxLength(value, 100);
                _descripcion = value;
            }
        }
        private Depto _depto;
        public Depto Depto
        {
            get { return _depto; }
            set
            {
                Validator.IsNotNull(value);
                _depto = value;
            }
        }
        private Categoria _categoria;
        public Categoria Categoria
        {
            get { return _categoria; }
            set
            {
                Validator.IsNotNull(value);
                _categoria = value;
            }
        }
        private EstadoSolicitud _estado;
        public EstadoSolicitud Estado
        {
            get { return _estado; }
            set
            {
                Validator.IsNotNull(value);
                _estado = value;
            }
        }

        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        protected Solicitud() { }

        public Solicitud(string descripcion, Depto depto, Categoria categoria, EstadoSolicitud estado, DateTime fechaInicio)
        {
            Id = Guid.NewGuid();
            _descripcion = descripcion;
            _depto = depto;
            _categoria = categoria;
            _estado = estado;
            FechaInicio = fechaInicio;
        }



        public Solicitud(Mantenimiento? perMan, string descripcion, Depto depto, Categoria categoria, EstadoSolicitud estado, DateTime fechaInicio)
        {
            Id = Guid.NewGuid();
            PerMan = perMan;
            _descripcion = descripcion;
            _depto = depto;
            _categoria = categoria;
            _estado = estado;
            FechaInicio = fechaInicio;

        }
        public Solicitud(string descripcion, Depto depto, Categoria categoria, DateTime fechaInicio)
        {
            Id = Guid.NewGuid();
            _descripcion = descripcion;
            _depto = depto;
            _categoria = categoria;
            _estado = EstadoSolicitud.Abierto;
            FechaInicio = fechaInicio;
        }


    }
}
