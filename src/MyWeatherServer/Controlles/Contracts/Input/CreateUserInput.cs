namespace Controllers.Contracts.Input;

internal class CreateUserInput
{
    public required string Email { get; set;}
    public required string Password { get; set;}
}