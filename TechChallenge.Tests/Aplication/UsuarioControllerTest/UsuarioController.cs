using Microsoft.AspNetCore.Mvc;
using Moq;
using TechChallenge.Api.Controllers;
using TechChallenge.Domain.Interfaces.Services;
using TechChallenge.Domain.Model.DTO;
using TechChallenge.Domain.Model.ViewModel;
using TechChallenge.Domain.Utils;

public class UsuarioControllerTests
{
    private readonly Mock<IUsuarioService> _usuarioServiceMock;
    private readonly UsuarioController _usuarioController;

    public UsuarioControllerTests()
    {
        _usuarioServiceMock = new Mock<IUsuarioService>();
        _usuarioController = new UsuarioController(_usuarioServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkWithListOfUsers()
    {
        // Arrange
        var usuarios = new List<UsuarioDTO>
        {
            new UsuarioDTO { Id = 1, Login = "User1" },
            new UsuarioDTO { Id = 2, Login = "User2" }
        };
        _usuarioServiceMock.Setup(service => service.GetAllAsync()).ReturnsAsync(usuarios);

        // Act
        var result = await _usuarioController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(usuarios, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnOkWithUser_WhenUserExists()
    {
        // Arrange
        var usuario = new UsuarioDTO { Id = 1, Login = "User1" };
        _usuarioServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync(usuario);

        // Act
        var result = await _usuarioController.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(usuario, okResult.Value);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _usuarioServiceMock.Setup(service => service.GetByIdAsync(1)).ReturnsAsync((UsuarioDTO)null);

        // Act
        var result = await _usuarioController.GetById(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CriaUsuario_ShouldReturnCreated_WhenUserIsValid()
    {
        // Arrange
        var usuario = new UsuarioInclusaoViewModel { Login = "NewUser", Senha = "beto$1234", PermissaoId=1};
        _usuarioServiceMock.Setup(service => service.AddAsync(usuario)).ReturnsAsync(Result.Success());

        // Act
        var result = await _usuarioController.CriaUsuario(usuario);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CriaUsuario_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _usuarioController.ModelState.AddModelError("Nome", "Required");
        var usuario = new UsuarioInclusaoViewModel();

        // Act
        var result = await _usuarioController.CriaUsuario(usuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void AlteraUsuario_ShouldReturnNoContent_WhenUpdateIsSuccessful()
    {
        // Arrange
        var usuario = new UsuarioAlteracaoViewModel { Id = 1, Login = "UpdatedUser" };
        _usuarioServiceMock.Setup(service => service.Update(usuario)).Returns(Result.Success());

        // Act
        var result = _usuarioController.AlteraUsuario(usuario);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void AlteraUsuario_ShouldReturnBadRequest_WhenModelStateIsInvalid()
    {
        // Arrange
        _usuarioController.ModelState.AddModelError("Nome", "Required");
        var usuario = new UsuarioAlteracaoViewModel();

        // Act
        var result = _usuarioController.AlteraUsuario(usuario);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void DeleteUsuario_ShouldReturnNoContent_WhenDeletionIsSuccessful()
    {
        // Arrange
        _usuarioServiceMock.Setup(service => service.Delete(1)).Returns(Result.Success());

        // Act
        var result = _usuarioController.DeleteUsuario(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteUsuario_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        _usuarioServiceMock.Setup(service => service.Delete(1)).Returns(Result.Failure("User not found"));

        // Act
        var result = _usuarioController.DeleteUsuario(1);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}
