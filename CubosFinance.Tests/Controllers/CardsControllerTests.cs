using CubosFinance.Api.Controllers;
using CubosFinance.Application.Abstractions.Services;
using CubosFinance.Application.DTOs.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace CubosFinance.Tests.Controllers;

public class CardsControllerTests
{
    private readonly Mock<ICardService> _cardServiceMock;
    private readonly CardsController _controller;

    public CardsControllerTests()
    {
        _cardServiceMock = new Mock<ICardService>();
        _controller = new CardsController(_cardServiceMock.Object);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        }, "mock"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    [Fact]
    public async Task GetAll_ShouldReturnOkResult()
    {
        var pagedResult = new PagedResponseDto<CardResponseDto>
        {
            Data = new List<CardResponseDto>(),
            Pagination = new PaginationDto { CurrentPage = 1, ItemsPerPage = 10 }
        };

        _cardServiceMock.Setup(s => s.GetAllByPersonAsync(It.IsAny<Guid>(), 1, 10))
            .ReturnsAsync(pagedResult);

        var result = await _controller.GetAll(1, 10) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }
}