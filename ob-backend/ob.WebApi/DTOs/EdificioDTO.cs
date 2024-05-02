using ob.Domain;

namespace ob.WebApi.DTOs
{
    public class EdificioDTO
    {
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Ubicacion { get; set; }
        public ConstructoraDTO Constructora { get; set; }
        public Decimal GastosComunes { get; set; }
        public List<DeptoDTO> Deptos { get; set; }

        public EdificioDTO()
        {
        }
        public EdificioDTO(Edificio edificio)
        {
            this.Nombre = edificio.Nombre;
            this.Direccion = edificio.Direccion;
            this.Ubicacion = edificio.Ubicación;
            this.Constructora = new ConstructoraDTO(edificio.EmpresaConstructora);
            this.GastosComunes = edificio.GastosComunes;
            this.Deptos = edificio.Deptos.Select(depto => new DeptoDTO(depto)).ToList();
        }

        public Edificio ToEntity()
        {
            return new Edificio(this.Nombre, this.Direccion, this.Ubicacion, this.Constructora.ToEntity(), this.GastosComunes, this.Deptos.Select(depto => depto.ToEntity()).ToList());
        }
    }
}
