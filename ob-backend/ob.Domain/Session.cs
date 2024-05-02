namespace ob.Domain;

public class Session
{
    public int Id { get; set; }
    public Guid AuthToken { get; set; }
    public Usuario Usuario { get; set; }

    public Session()
    {
        AuthToken = Guid.NewGuid();
    }
}