using CubosFinance.Api.Controllers;
using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.People;
using CubosFinance.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CubosFinance.Tests.Controllers;

public class PersonControllerTests
{
    private readonly Mock<IPersonService> _personServiceMock;
    private readonly PersonController _controller;

    public PersonControllerTests()
    {
        _personServiceMock = new Mock<IPersonService>();
        _controller = new PersonController(_personServiceMock.Object);
    }

    [Fact]
    public async Task Create_ShouldReturnConflict_WhenDocumentDuplicated()
    {
        var dto = new CreatePersonDto { Document = "12345678900", Name = "John" };

        _personServiceMock.Setup(s => s.CreateAsync(dto))
            .ThrowsAsync(new DuplicatedDocumentException("Duplicated document"));

        await Assert.ThrowsAsync<DuplicatedDocumentException>(() => _controller.Create(dto));
    }
}