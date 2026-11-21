namespace Domain.model;

public class Belt
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Rank Rank { get; set; }
    public string ColorHex { get; set; }
    public double RequiredScore { get; set; }
}

public enum Rank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
}