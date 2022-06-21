using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using ShanesAPP.Controllers;
using ShanesAPP.Infrastructure;
using ShanesAPP.Infrastructure.Interfaces;
using ShanesAPP.Model;
using ShanesAPP.Services.Interfaces;


namespace ShanesAPP.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {
        private Mock<ISessionService> _mockSessionService;
        private Mock<IGoogleApiService> _mockGoogleApiService;
        private Mock<IAuthorizationService> _mockAuthorizationService;
        private Mock<ILogger> _mockLogger;
        private HomeController _homeController;

        private const string State = "1234";
        private const string Code = "abcd";
        private Dictionary<string, StringValues> RequestParamsDictionary = new()
        {
            { "state", State },
            { "code", Code },
        };

        [SetUp]
        public void SetUp()
        {
            _mockLogger = new Mock<ILogger>();
            _mockGoogleApiService = new Mock<IGoogleApiService>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();
            _mockSessionService = new Mock<ISessionService>();
            _homeController = new HomeController(_mockAuthorizationService.Object,
                _mockGoogleApiService.Object,
                _mockSessionService.Object,
                _mockLogger.Object);
        }

        [Test]
        public void Callback_ValidResponse_ReturnsValidResultModel()
        {
            // Arrange
            var tokenResultModel = new TokenResultModel
            {
                IsValid = true
            };


            var context = new DefaultHttpContext();
            _homeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context
            };
            _homeController.Request.Query = new QueryCollection(RequestParamsDictionary);

            _mockAuthorizationService
                .Setup(s => s.IsValidateState(_homeController.ControllerContext.HttpContext, State))
                .Returns(true);
            _mockAuthorizationService
                .Setup(s => s.GetToken(_homeController.ControllerContext.HttpContext, Code))
                .Returns(Task.FromResult(tokenResultModel));


            // Act
            var result = _homeController.Callback();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(string.Empty));
            _mockAuthorizationService.Verify(v => v.IsValidateState(_homeController.ControllerContext.HttpContext, State), Times.Once);
            _mockAuthorizationService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext, Code), Times.Once);
        }

        [Test]
        public void Callback_InValidToken_ReturnsTokenErrorMessage()
        {
            // Arrange
            var tokenResultModel = new TokenResultModel
            {
                IsValid = false
            };

            var context = new DefaultHttpContext();
            _homeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context
            };
            _homeController.Request.Query = new QueryCollection(RequestParamsDictionary);

            _mockAuthorizationService
                .Setup(s => s.IsValidateState(_homeController.ControllerContext.HttpContext, State))
                .Returns(true);

            _mockAuthorizationService
                .Setup(s => s.GetToken(_homeController.ControllerContext.HttpContext, Code))
                .Returns(Task.FromResult(tokenResultModel));

            // Act
            var result = _homeController.Callback();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.TokenErrorMessage));
            _mockAuthorizationService.Verify(v => v.IsValidateState(_homeController.ControllerContext.HttpContext, State), Times.Once);
            _mockAuthorizationService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext, Code), Times.Once);
        }

        [Test]
        public void Callback_InValidCode_ReturnsNoCodeReturned()
        {
            // Arrange
            var requestParamsDictionary = new Dictionary<string, StringValues>()
            {
                { "state", State }
            };

            var context = new DefaultHttpContext();
            _homeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context
            };
            _homeController.Request.Query = new QueryCollection(requestParamsDictionary);

            _mockAuthorizationService
                .Setup(s => s.IsValidateState(_homeController.ControllerContext.HttpContext, State))
                .Returns(true);

            // Act
            var result = _homeController.Callback();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.NoCodeReturned));
            _mockAuthorizationService.Verify(v => v.IsValidateState(_homeController.ControllerContext.HttpContext, State), Times.Once);
            _mockAuthorizationService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext, Code), Times.Never);
        }

        [Test]
        public void Callback_InValidState_ReturnsIncorrectState()
        {
            // Arrange
            var requestParamsDictionary = new Dictionary<string, StringValues>()
            {
                { "state", State }
            };

            var context = new DefaultHttpContext();
            _homeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context
            };
            _homeController.Request.Query = new QueryCollection(requestParamsDictionary);

            _mockAuthorizationService
                .Setup(s => s.IsValidateState(_homeController.ControllerContext.HttpContext, State))
                .Returns(false);

            // Act
            var result = _homeController.Callback();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.IncorrectStateReturned));
            _mockAuthorizationService.Verify(v => v.IsValidateState(_homeController.ControllerContext.HttpContext, State), Times.Once);
            _mockAuthorizationService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext, Code), Times.Never);
        }

        [Test]
        public void Callback_ThrowsException_ExceptionIsHandled()
        {
            // Arrange
            var context = new DefaultHttpContext();
            _homeController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext()
            {
                HttpContext = context
            };
            _homeController.Request.Query = new QueryCollection(RequestParamsDictionary);

            _mockAuthorizationService
                .Setup(s => s.IsValidateState(_homeController.ControllerContext.HttpContext, State))
                .Throws(new Exception());

            // Act
            var result = _homeController.Callback();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.UnexpectedException));
            _mockLogger.Verify(v => v.Log(It.IsAny<string>()), Times.Once);
            _mockAuthorizationService.Verify(v => v.IsValidateState(_homeController.ControllerContext.HttpContext, State), Times.Once);
            _mockAuthorizationService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext, Code), Times.Never);
        }

        [Test]
        public void GetUserInfo_ThrowsException_ExceptionIsHandled()
        {
            // Arrange

            _mockSessionService
                .Setup(s => s.GetToken(_homeController.ControllerContext.HttpContext))
                .Throws(new Exception());

            // Act
            var result = _homeController.GetUserInfo();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.UnexpectedException));
            _mockLogger.Verify(v => v.Log(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void GetUserInfo_InValidToken_ReturnsTokenErrorMessage()
        {
            // Arrange

            _mockAuthorizationService
                .Setup(s => s.IsTokenValid(It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = _homeController.GetUserInfo();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.TokenErrorMessage));
            _mockSessionService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext), Times.Once);
            _mockAuthorizationService.Verify(v => v.IsTokenValid(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task GetUserInfo_ValidToken_ReturnReturnsGoogleUserModel()
        {
            // Arrange

            _mockAuthorizationService
                .Setup(s => s.IsTokenValid(It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = await _homeController.GetUserInfo();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(string.Empty));
            _mockSessionService.Verify(v => v.GetToken(_homeController.ControllerContext.HttpContext), Times.Once);
            _mockAuthorizationService.Verify(v => v.IsTokenValid(It.IsAny<string>()), Times.Once);
            _mockGoogleApiService.Verify(v => v.GetUserInfo(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void LogOut_ClearsToken_ReturnsNoErrors()
        {
            // Act
            var result = _homeController.LogOut();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(string.Empty));
        }

        [Test]
        public void LogOut_ClearsToken_HandlesException()
        {
            // Arrange
            _mockSessionService
                .Setup(s => s.ClearTokens(_homeController.ControllerContext.HttpContext))
                .Throws(new Exception());

            // Act
            var result = _homeController.LogOut();

            // Assert
            Assert.That(_homeController.ErrorMessage, Is.EqualTo(Constants.LogOutErrorMessage));
            _mockLogger.Verify(v => v.Log(It.IsAny<string>()), Times.Once);
        }
    }
}
