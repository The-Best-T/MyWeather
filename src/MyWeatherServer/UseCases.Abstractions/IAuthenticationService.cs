namespace UseCases.Abstractions;

public interface IAuthenticationService
{
    Task<string> CreateTokenAsync(
        string userEmail);
}