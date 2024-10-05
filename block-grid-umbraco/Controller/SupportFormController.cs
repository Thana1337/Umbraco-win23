using block_grid_umbraco.Dtos;
using block_grid_umbraco.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace block_grid_umbraco.Controller
{
    public class SupportFormController : SurfaceController
    {
        private readonly EmailService _emailService;

        public SupportFormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, EmailService emailService)
            : base(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> HandleSubmit(string email)
        {
            ViewData["error_email"] = string.IsNullOrEmpty(email) || !IsValidEmail(email);

            if (ViewData.Values.OfType<bool>().Contains(true))
            {
                ViewData["email"] = email; 
                return CurrentUmbracoPage();
            }

            try
            {
                var emailDto = new EmailDto
                {
                    Email = email
                };

                await _emailService.SendDataToAzureFunction(emailDto);
                TempData["success"] = "Thank you for reaching out! We will contact you soon.";
            }
            catch (HttpRequestException)
            {
                TempData["error"] = $"There was an issue sending your message. Please try again.";
            }

            string currentUrlWithAnchor = $"{UmbracoContext?.PublishedRequest?.PublishedContent?.Url()}#form-section";
            return Redirect(currentUrlWithAnchor);
        }

        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
