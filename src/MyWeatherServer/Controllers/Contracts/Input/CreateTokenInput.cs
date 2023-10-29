namespace Controllers.Contracts.Input;

public class CreateTokenInput
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}