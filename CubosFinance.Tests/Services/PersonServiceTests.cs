using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.Exceptions;
using CubosFinance.Application.Services;
using CubosFinance.Domain.Abstractions.Repositories;
using CubosFinance.Domain.Entities;
using Moq;

namespace CubosFinance.Tests.Services;

public class PersonServiceTests
{
    private readonly Mock<IPersonRepository> _repositoryMock;
    private readonly PersonService _service;

    public PersonServiceTests()
    {
        _repositoryMock = new Mock<IPersonRepository>();
        _service = new PersonService(_repositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_WhenDataIsValid()
    {
        var dto = new CreatePersonDto
        {
            Name = "Bruno Piccoli",
            Document = "12345678900",
            Password = "SenhaForte123"
        };

        _repositoryMock.Setup(r => r.ExistsByDocumentAsync(dto.Document))
            .ReturnsAsync(false);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Person>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateAsync(dto);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.Document, result.Document);
        Assert.True(result.CreatedAt > DateTime.MinValue);
        Assert.True(result.UpdatedAt > DateTime.MinValue);
    }


    [Fact]
    public async Task CreateAsync_WhenDocumentAlreadyExists()
    {
        var dto = new CreatePersonDto
        {
            Name = "Bruno",
            Document = "12345678900",
            Password = "SenhaSegura123"
        };

        _repositoryMock
            .Setup(r => r.ExistsByDocumentAsync(dto.Document))
            .ReturnsAsync(true);

        await Assert.ThrowsAsync<DuplicatedDocumentException>(() => _service.CreateAsync(dto));
    }
}
