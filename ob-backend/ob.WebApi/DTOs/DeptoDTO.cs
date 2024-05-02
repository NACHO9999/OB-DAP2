using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class DeptoDTO
    {
        public int Numero { get; set; }
        public int Piso { get; set; }
        public int CantidadBanos { get; set; }
        public DuenoDTO Dueno { get; set; }
        public int CantidadCuartos { get; set; }
        public bool ConTerraza { get; set; }
        public string EdificioNombre { get; set; }
        public string EdificioDireccion { get; set; }

        public DeptoDTO(Depto depto)
        {
            this.Numero = depto.Numero;
            this.Piso = depto.Piso;
            this.CantidadBanos = depto.CantidadBanos;
            this.CantidadCuartos = depto.CantidadCuartos;
            this.Dueno = new DuenoDTO(depto.Dueno);
            this.ConTerraza = depto.ConTerraza;
            this.EdificioNombre = depto.EdificioNombre;
            this.EdificioDireccion = depto.EdificioDireccion;
        }
        public DeptoDTO() { }

        public Depto ToEntity()
        {
            return new Depto( this.Piso, this.Numero, new Dueno(this.Dueno.Nombre, this.Dueno.Apellido, this.Dueno.Email), this.CantidadCuartos, this.CantidadBanos, this.ConTerraza,this.EdificioNombre, this.EdificioDireccion);
        }
    }
}
