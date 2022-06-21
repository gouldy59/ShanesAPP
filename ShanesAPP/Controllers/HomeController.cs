using Microsoft.AspNetCore.Mvc;
using ShanesAPP.Infrastructure;
using ShanesAPP.Model;
using ShanesAPP.Services.Interfaces;
using ILogger = ShanesAPP.Infrastructure.Interfaces.ILogger;

namespace ShanesAPP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly IGoogleApiService _googleApiService;
        private readonly ISessionService _sessionService;
        private readonly ILogger _logger;

        [TempData]
        public string ErrorMessage { get; set; } = string.Empty;

        public HomeController(IAuthorizationService authorizationService,
            IGoogleApiService googleApiService,
            ISessionService sessionService,
            ILogger logger)
        {
            _authorizationService = authorizationService;
            _googleApiService = googleApiService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public IActionResult Index() {
            HomePageModel model = new()
            {
                IsValid = false
            };

            var token = _sessionService.GetToken(HttpContext);
            if (_authorizationService.IsTokenValid(token))
            {
                model.IsValid = true;
            }
          
            return View("index", model);
        }

        public async Task<IActionResult> Callback()
        {
            try
            {
                var state = Request.Query["state"];
                if (!_authorizationService.IsValidateState(HttpContext, state))
                {
                    return RedirectWithErrorMessage(Constants.IncorrectStateReturned);
                }

                var code = Request.Query["code"];
                if (string.IsNullOrEmpty(code))
                {
                    return RedirectWithErrorMessage(Constants.NoCodeReturned);
                }

                var tokenResultModel = await _authorizationService.GetToken(HttpContext, code);
                if (!tokenResultModel.IsValid)
                {
                    return RedirectWithErrorMessage(Constants.TokenErrorMessage);
                }
            }
            catch(Exception ex)
            {
                _logger.Log(ex.Message);
                ErrorMessage = Constants.UnexpectedException;
            }

            return RedirectToAction("index", "Home");
        }

        public async Task<IActionResult> GetUserInfo()
        {
            var googleUserResultModel = new GoogleUserResultModel();

            try
            {
                var accessToken = _sessionService.GetToken(HttpContext);
                if (_authorizationService.IsTokenValid(accessToken))
                {
                    googleUserResultModel = await _googleApiService.GetUserInfo(accessToken);
                }
                else
                {
                    return RedirectWithErrorMessage(Constants.TokenErrorMessage);
                }
            }
            catch(Exception ex)
            {
                _logger.Log(ex.Message);
                ErrorMessage = Constants.UnexpectedException;
            }

            return PartialView("_UserInfo", googleUserResultModel);
        }

        public IActionResult LogOut()
        {
            try
            {
                _sessionService.ClearTokens(HttpContext);
            }
            catch(Exception ex)
            {
                _logger.Log(ex.Message);
                return RedirectWithErrorMessage(Constants.LogOutErrorMessage);
            }
            return RedirectToAction("index", "Home");
        }

        public IActionResult Authorize()
        {
            return Redirect(_authorizationService.GetUri(HttpContext));
        }

        private IActionResult RedirectWithErrorMessage(string errorMessage)
        {
            ErrorMessage = errorMessage;
            return RedirectToAction("index", "Home");
        }
    }
}