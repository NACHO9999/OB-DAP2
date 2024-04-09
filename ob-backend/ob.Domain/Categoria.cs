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
    }
    
}