namespace Domain.model;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BeltId { get; set; }
    public Belt Belt { get; set; }

    public string PasswordHash { get; set; }
}