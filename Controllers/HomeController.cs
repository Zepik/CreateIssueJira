using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CreateIssueJira.Model;
using CreateIssueJira.Services;
using CreateIssueJira.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CreateIssueJira.Controllers
{
    public class HomeController : Controller
    {

        private JiraIssueService _issueService;
        private IConfigurationRoot _Configuration;
        public HomeController(JiraIssueService issueService, IConfigurationRoot Configuration)
         {
             _issueService = issueService;
             _Configuration = Configuration;
         }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string Summary, string Description, string Reporter, IList<IFormFile> files)
        {
            JSON_issue issueToCreate = new JSON_issue(_Configuration["Issue:projectId"], Summary, Description, _Configuration["Issue:issueType"], Reporter);
            IssueCreateResponse respone = await _issueService.CreateIssueJira(issueToCreate);
            List<string> filesnames;
            if(files.Count > 0)
            {
                filesnames = _issueService.UploadAttachments(files);  
                var respone2 = await _issueService.SendAttachmentsToIssueJira(filesnames, respone.key);
            }
            return View("IssueCreated", respone);
        }
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();

            ViewData["statusCode"] = HttpContext.Response.StatusCode;

            return View();
        }
    }
}