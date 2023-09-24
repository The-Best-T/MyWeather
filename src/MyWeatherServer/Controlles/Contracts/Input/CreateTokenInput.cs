namespace Controllers.Contracts.Input;

internal class CreateTokenInput
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}