namespace TechChallenge.Usuarios.Api.Domain.Interfaces
{
    public interface IPasswordService
    {
        string GerarHash(string senha);
        bool VerificarSenha(string senhaDigitada, string hashSalvo);
    }
}
