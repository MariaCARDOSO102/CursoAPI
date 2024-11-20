using AlunosApi.Controllers;
using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AlunosApi.Tests
{
    public class AlunosControllerTests
    {
        private readonly Mock<IAlunoService> _mockAlunoService;
        private readonly AlunosController _controller;

        public AlunosControllerTests()
        {
            _mockAlunoService = new Mock<IAlunoService>();
            _controller = new AlunosController(_mockAlunoService.Object);
        }

        [Fact]
        public async Task GetAlunos_ReturnsOkResult_WithListOfAlunos()
        {
            // Arrange
            var alunos = new List<Aluno>
            {
                new Aluno { Id = 1, Nome = "João" },
                new Aluno { Id = 2, Nome = "Maria" }
            }.AsEnumerable();

            _mockAlunoService.Setup(service => service.GetAlunos()).ReturnsAsync(alunos);

            // Act
            var result = await _controller.GetAlunos();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAlunos = Assert.IsAssignableFrom<IEnumerable<Aluno>>(okResult.Value);
            Assert.Equal(2, returnAlunos.Count());
        }

        [Fact]
        public async Task GetAlunosByName_ReturnsNotFound_WhenNoAlunosMatch()
        {
            // Arrange
            string nome = "Inexistente";
            _mockAlunoService.Setup(service => service.GetAlunosByNome(nome)).ReturnsAsync(Enumerable.Empty<Aluno>());

            // Act
            var result = await _controller.GetAlunosByName(nome);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal($"Não existem alunos com o critério {nome}", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAluno_ReturnsOkResult_WithAluno()
        {
            // Arrange
            int id = 1;
            var aluno = new Aluno { Id = id, Nome = "João" };
            _mockAlunoService.Setup(service => service.GetAluno(id)).ReturnsAsync(aluno);

            // Act
            var result = await _controller.GetAluno(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnAluno = Assert.IsType<Aluno>(okResult.Value);
            Assert.Equal(id, returnAluno.Id);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtRoute_WithAluno()
        {
            // Arrange
            var aluno = new Aluno { Id = 1, Nome = "João" };
            _mockAlunoService.Setup(service => service.CreateAluno(aluno)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(aluno);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal("GetAluno", createdAtRouteResult.RouteName);
            Assert.Equal(aluno.Id, ((Aluno)createdAtRouteResult.Value).Id);
        }

        [Fact]
        public async Task Edit_ReturnsOkResult_WhenAlunoIsUpdated()
        {
            // Arrange
            var aluno = new Aluno { Id = 1, Nome = "João Atualizado" };
            _mockAlunoService.Setup(service => service.UpdateAluno(aluno)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(aluno.Id, aluno);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Aluno com id={aluno.Id} foi atualizado com sucesso", okResult.Value);
        }

        [Fact]
        public async Task Delete_ReturnsOkResult_WhenAlunoIsDeleted()
        {
            // Arrange
            int id = 1;
            var aluno = new Aluno { Id = id, Nome = "João Atualizado" };
            _mockAlunoService.Setup(service => service.GetAluno(id)).ReturnsAsync(aluno);
            _mockAlunoService.Setup(service => service.DeleteAluno(aluno)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result); // Verifica se o resultado é OkObjectResult
            Assert.Equal($"Aluno de id={id} foi excluido com sucesso", okResult.Value); // Verifica a mensagem de sucesso
        }

    }
}