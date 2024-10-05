﻿using block_grid_umbraco.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Umbraco.Cms.Core.Cache;
using Umbraco.Cms.Core.Logging;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Infrastructure.Persistence;
using Umbraco.Cms.Web.Website.Controllers;

namespace block_grid_umbraco.Controller;

public class ContactMessageFormController(IUmbracoContextAccessor umbracoContextAccessor, IUmbracoDatabaseFactory databaseFactory, ServiceContext services, AppCaches appCaches, IProfilingLogger profilingLogger, IPublishedUrlProvider publishedUrlProvider) : SurfaceController(umbracoContextAccessor, databaseFactory, services, appCaches, profilingLogger, publishedUrlProvider)
{
    public IActionResult HandleSubmit(ContactFormModel form)
    {
        ViewData["error_name"] = string.IsNullOrEmpty(form.Name);
        ViewData["error_email"] = string.IsNullOrEmpty(form.Email);
        ViewData["error_message"] = string.IsNullOrEmpty(form.Message);

        if (ViewData.Values.OfType<bool>().Contains(true))
        {
            ViewData["name"] = form.Name;
            ViewData["email"] = form.Email;
            ViewData["message"] = form.Message;
            return CurrentUmbracoPage();
        }

        TempData["success"] = "Thank you, we will contact you very soon";
        string currentUrlWithAnchor = $"{UmbracoContext.PublishedRequest.PublishedContent.Url()}#form-section";
        return Redirect(currentUrlWithAnchor);
    }
}
