namespace TechChallenge.Domain.Interfaces.Services
{
    public interface IPasswordService
    {
        string GerarHash(string senha);
        bool VerificarSenha(string senhaDigitada, string hashSalvo);
    }
}
