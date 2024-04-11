using Enums;
namespace ob.Domain

{
    public class Solicitud
    {
        private Mantenimiento _perMan;
        public Mantenimiento PerMan
        {
            get { return _perMan; }
            set
            {
                Validator.IsNotNull(value);
                _perMan = value;
            }
        }
        private string _descripcion;
        public string Descripcion {
            get { return _descripcion; }
            set
            {
                Validator.ValidateString(value);
                Validator.ValidateStringMaxLength(value, 100);
                _descripcion = value;
            }
        }
        private Depto _depto;
        public Depto Depto {
            get { return _depto; }
            set
            {
                Validator.IsNotNull(value);
                _depto = value;
            }
        }
        private EstadoSolicitud _estado;
        public EstadoSolicitud Estado {
            get { return _estado; }
            set
            {
                Validator.IsNotNull(value);
                _estado = value;
            }
        }
        
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public Solicitud(Mantenimiento perMan, string descripcion, Depto depto, EstadoSolicitud estado, DateTime fechaInicio)
        {
            _perMan = perMan;
            _descripcion = descripcion;
            _depto = depto;
            _estado = estado;
            FechaInicio = fechaInicio;
            
        }


    }
}