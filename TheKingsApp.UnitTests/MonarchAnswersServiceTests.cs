using Moq;
using TheKingsApp.Core.Entities;
using TheKingsApp.Core.Interfaces;
using TheKingsApp.UseCases.Services;

namespace TheKingsApp.UnitTests
{
    public class MonarchAnswersServiceTests
    {
        private readonly Mock<IMonarchService> _monarchServiceMock;
        private readonly MonarchAnswersService _service;

        public MonarchAnswersServiceTests()
        {
            _monarchServiceMock = new Mock<IMonarchService>();
            _service = new MonarchAnswersService(_monarchServiceMock.Object);
        }

        [Fact]
        public async Task GetAnswersAboutMonarchsAsync_ReturnsCorrectAnswers()
        {
            // Arrange
            var monarchs = new List<Monarchs>
        {
            new Monarchs { Id = 1, Name = "Henry VII", Country = "England", House = "Tudor", Years = "1485-1509" },
            new Monarchs { Id = 2, Name = "Henry VIII", Country = "England", House = "Tudor", Years = "1509-1547" },
            new Monarchs { Id = 3, Name = "Elizabeth I", Country = "England", House = "Tudor", Years = "1558-1603" },
            new Monarchs { Id = 4, Name = "James I", Country = "England", House = "Stuart", Years = "1603-1625" }
        };

            _monarchServiceMock.Setup(m => m.GetMonarchsAsync()).ReturnsAsync(monarchs);

            // Act
            var result = await _service.GetAnswersAboutMonarchsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(4, result.Total);
            Assert.Equal("Tudor", result.HouseName);
            Assert.Equal(107, result.HouseYears); // Henry VII (24), Henry VIII (38) + Elizabeth (45)
            Assert.Equal("Elizabeth I", result.MonarchName);
            Assert.Equal(45, result.MonarchYears);
            Assert.Equal("Henry", result.CommonName);
        }

        [Fact]
        public async Task GetAnswersAboutMonarchsAsync_ThrowsArgumentNullException_WhenMonarchListIsNull()
        {
            // Arrange
            _monarchServiceMock
                .Setup(m => m.GetMonarchsAsync())
                .ReturnsAsync((List<Monarchs>?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _service.GetAnswersAboutMonarchsAsync());
        }
    }
}