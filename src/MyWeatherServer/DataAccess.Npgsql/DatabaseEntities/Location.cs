namespace DataAccess.Npgsql.DatabaseEntities;

public class Location
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public required string UserId { get; set; }
}