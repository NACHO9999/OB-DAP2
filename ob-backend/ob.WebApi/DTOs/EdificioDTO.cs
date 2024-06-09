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
        public Edificio ToEntity()
        {
            return new Edificio(this.Nombre, this.Direccion, this.Ubicacion, this.Constructora.ToEntity(), this.GastosComunes, this.Deptos.Select(depto => depto.ToEntity()).ToList());
            
        }

        public EdificioDTO(Edificio edificio)
        {
            if (edificio == null)
            {
                throw new ArgumentNullException(nameof(edificio), "The provided edificio is null.");
            }

            this.Nombre = edificio.Nombre ?? string.Empty;
            this.Direccion = edificio.Direccion ?? string.Empty;
            this.Ubicacion = edificio.Ubicación ?? string.Empty;

            if (edificio.EmpresaConstructora == null)
            {
                Console.WriteLine("EdificioDTO: EmpresaConstructora is null.");
                throw new ArgumentNullException(nameof(edificio.EmpresaConstructora), "The EmpresaConstructora is null.");
            }
            this.Constructora = new ConstructoraDTO(edificio.EmpresaConstructora);

            this.GastosComunes = edificio.GastosComunes;

            if (edificio.Deptos == null)
            {
                Console.WriteLine("EdificioDTO: Deptos is null, initializing to an empty list.");
                this.Deptos = new List<DeptoDTO>();
            }
            else
            {
                Console.WriteLine($"EdificioDTO: Deptos has {edificio.Deptos.Count} items.");
                this.Deptos = edificio.Deptos.Select(depto => new DeptoDTO(depto)).ToList();
            }

            // Log the state of the object
            Console.WriteLine($"Created EdificioDTO: Nombre={this.Nombre}, Direccion={this.Direccion}, Ubicacion={this.Ubicacion}, GastosComunes={this.GastosComunes}");
        }
    }
}
