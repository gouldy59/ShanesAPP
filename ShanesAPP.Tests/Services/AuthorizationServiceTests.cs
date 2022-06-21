using Microsoft.AspNetCore.Http;
using Moq;
using ShanesAPP.Infrastructure.Interfaces;
using ShanesAPP.Services;
using ShanesAPP.Services.Interfaces;
using System.Text;

namespace ShanesAPP.Tests.Services
{
    [TestFixture]

    public class AuthorizationServiceTests
    {
        private Mock<IAuthClient> _mockApiClient;
        private Mock<IAppSettings> _mockAppSettings;
        private Mock<ISessionService> _mockSessionService;


        [SetUp]
        public void SetUp()
        {
            _mockApiClient = new Mock<IAuthClient>();
            _mockAppSettings = new Mock<IAppSettings>();
            _mockSessionService = new Mock<ISessionService>();
        }

        [Test]
        public async Task GetToken_InvalidResponse_ReturnsInValidResultModel()
        {
            // Arrange

            _mockApiClient.Setup(s => s.GetTokenAsync(It.IsAny<string>())).Returns(Task.FromResult(""));
            var authorizationService = new AuthorizationService(_mockAppSettings.Object,
                _mockApiClient.Object,
                _mockSessionService.Object);

            // Act
            var result = await authorizationService.GetToken(new DefaultHttpContext(), "code");

            // Assert
            Assert.IsFalse(result.IsValid);
        }

        [Test]
        public async Task GetToken_ValidResponse_ReturnsValidResultModel()
        {
            // Arrange
            var defaultHttpContext = new DefaultHttpContext();
            _mockApiClient.Setup(s => s.GetTokenAsync(It.IsAny<string>())).Returns(Task.FromResult("Token"));
            var authorizationService = new AuthorizationService(_mockAppSettings.Object,
                _mockApiClient.Object,
                _mockSessionService.Object);

            // Act
            var result = await authorizationService.GetToken(defaultHttpContext, "code");

            // Assert
            Assert.IsTrue(result.IsValid);
            _mockApiClient.Verify(v => v.GetTokenAsync("code"), Times.Once);
            _mockSessionService.Verify(v => v.SetToken(defaultHttpContext, "Token"), Times.Once);
        }

    }
}
