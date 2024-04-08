namespace ob.Domain
{
    public class Categoria
    {
        public int Id { get; private set; }
        public string Nombre { get; private set; }

        public Categoria(string nombre)
        {
            Nombre = nombre;
        }

        public void CambiarNombre(string nuevoNombre)
        {
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                Nombre = nuevoNombre;
            }
            else
            {
                throw new ArgumentException("El nombre de la categoría no puede estar vacío", nameof(nuevoNombre));
            }
        }
    }
    
}