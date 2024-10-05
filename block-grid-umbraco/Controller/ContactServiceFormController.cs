using block_grid_umbraco.Dtos;
using block_grid_umbraco.Models;
using block_grid_umbraco.Services;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace block_grid_umbraco.Controller;

public class ContactServiceFormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider, EmailService emailService) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    private readonly EmailService _emailService = emailService;
    [HttpPost]
    public async Task<IActionResult> HandleSubmit(ContactFormModel form)
    {
        ViewData["error_name"] = string.IsNullOrEmpty(form.Name);
        ViewData["error_email"] = string.IsNullOrEmpty(form.Email);
        ViewData["error_phone"] = string.IsNullOrEmpty(form.Phone);
        ViewData["error_service"] = string.IsNullOrEmpty(form.Service);

        if (ViewData.Values.OfType<bool>().Contains(true))
        {
            ViewData["name"] = form.Name;
            ViewData["email"] = form.Email;
            ViewData["phone"] = form.Phone;
            ViewData["service"] = form.Service;
            return CurrentUmbracoPage();
        }

        try
        {

            var emailDto = new EmailDto
            {
                Name = form.Name,
                Email = form.Email,
                Phone = form.Phone,
                Service = form.Service
            };

            await _emailService.SendDataToAzureFunction(emailDto);

            TempData["success"] = "Thank you, we will contact you very soon.";
        }
        catch (HttpRequestException ex)
        {
            TempData["error"] = $"There was an issue sending your message. Please try again. Error: {ex.Message}";
        }

        string currentUrlWithAnchor = $"{UmbracoContext?.PublishedRequest?.PublishedContent?.Url()}#form-section";
        return Redirect(currentUrlWithAnchor);
    }

}
