using AlunosApi.Controllers;
using AlunosApi.Services;
using AlunosApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AlunosApi.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IAuthenticate> _mockAuthentication;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockAuthentication = new Mock<IAuthenticate>();

            // Configurações simuladas para chave e configuração JWT
            _mockConfiguration.Setup(config => config["JWT:key"]).Returns("chave_secreta_teste");
            _mockConfiguration.Setup(config => config["Jwt:Issuer"]).Returns("issuer_teste");
            _mockConfiguration.Setup(config => config["Jwt:Audience"]).Returns("audience_teste");

            _controller = new AccountController(_mockConfiguration.Object, _mockAuthentication.Object);
        }

        [Fact]
        public async Task Login_ReturnsOkResult_WithUserToken_WhenCredentialsAreValid()
        {
            // Arrange: prepara as credenciais e simula autenticação válida
            var loginModel = new LoginModel { Email = "teste@dominio.com", Password = "SenhaValida123" };
            _mockAuthentication.Setup(auth => auth.Authenticate(loginModel.Email, loginModel.Password)).ReturnsAsync(true);

            // Act: executa o login
            var result = await _controller.Login(loginModel);

            // Assert: verifica o resultado de sucesso
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var userToken = Assert.IsType<UserToken>(okResult.Value);

            // Verifica se o token e data de expiração são válidos
            Assert.NotNull(userToken.Token);
            Assert.True(userToken.Expiration > DateTime.UtcNow);
        }
    }
}
