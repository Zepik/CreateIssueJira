using System;
using System.Threading.Tasks;
using CreateIssueJira.Model;
using CreateIssueJira.Services;
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
        public async Task<IActionResult> Index(string Summary, string Description, string Reporter)
        {
            JSON_issue issueToCreate = new JSON_issue(_Configuration["Issue:projectId"], Summary, Description, _Configuration["Issue:issueType"], Reporter);
            var respone = await _issueService.CreateIssueJira(issueToCreate);
            
            return View("IssueCreated", respone);
        }
    }
}