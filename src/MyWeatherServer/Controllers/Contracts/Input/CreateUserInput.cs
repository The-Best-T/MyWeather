namespace Controllers.Contracts.Input;

public class CreateUserInput
{
    public required string Email { get; set;}
    public required string Password { get; set;}
}