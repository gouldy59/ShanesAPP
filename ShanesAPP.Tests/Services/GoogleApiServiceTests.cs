using Moq;
using ShanesAPP.Services;
using ShanesAPP.Services.Interfaces;
using System.Text;

namespace ShanesAPP.Tests.Services
{
    [TestFixture]

    public class GoogleApiServiceTests
    {
        private Mock<IGoogleApiClient> _mockGoogleApiClient;

        [SetUp]
        public void SetUp()
        {
            _mockGoogleApiClient = new Mock<IGoogleApiClient>();
        }

        [Test]
        public async Task GetUserInfo_InvalidResponse_ReturnsInValidModel()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway);

            _mockGoogleApiClient.Setup(s => s.GetUserInfo(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
            var googleApiService = new GoogleApiService(_mockGoogleApiClient.Object);

            // Act
            var result = await googleApiService.GetUserInfo("test");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Error Code: 502 : Error Reason: Bad Gateway", result.ErrorMessage);
        }

        [Test]
        public async Task GetUserInfo_InvalidContent_ReturnsInValidModel()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);


            _mockGoogleApiClient.Setup(s => s.GetUserInfo(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
            var googleApiService = new GoogleApiService(_mockGoogleApiClient.Object);

            // Act
            var result = await googleApiService.GetUserInfo("test");

            // Assert
            Assert.IsFalse(result.IsValid);
            Assert.That(result.ErrorMessage, Is.EqualTo("No Content was returned"));
        }

        [Test]
        public async Task GetUserInfo_ValidContent_ReturnsIsValidModel()
        {
            // Arrange
            var httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Accepted)
            {
                Content = new StreamContent(GetUserInfoStream())
            };

            _mockGoogleApiClient.Setup(s => s.GetUserInfo(It.IsAny<string>())).Returns(Task.FromResult(httpResponseMessage));
            var googleApiService = new GoogleApiService(_mockGoogleApiClient.Object);

            // Act
            var result = await googleApiService.GetUserInfo("test");

            // Assert
            Assert.IsTrue(result.IsValid);
            Assert.That(result.Name, Is.EqualTo("shane2 gould2"));
            Assert.That(result.Id, Is.EqualTo("109759607161687292677"));
        }

        private Stream GetUserInfoStream()
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"Resources/UserInfo.json"));
            //byte[] byteArray = Encoding.ASCII.GetBytes(contents);
            return new MemoryStream(byteArray);
        }
    }
}
