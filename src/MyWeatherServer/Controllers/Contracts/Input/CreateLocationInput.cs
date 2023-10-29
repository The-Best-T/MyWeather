namespace Controllers.Contracts.Input;

public class CreateLocationInput
{
    public required string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}