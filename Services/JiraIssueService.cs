using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CreateIssueJira.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using CreateIssueJira.ViewModels;

namespace CreateIssueJira.Services
{

    public class JiraIssueService
    {
        private IConfigurationRoot _Configuration;
        private readonly ILogger _logger;
        public JiraIssueService(IConfigurationRoot Configuration,  ILogger<JiraIssueService> logger)
        {
            _Configuration = Configuration;
            _logger = logger;
        }

        public async Task<IssueCreateResponse> CreateIssueJira(JSON_issue issue)
        {
            var client = new HttpClient();
            var Credentials = Encoding.ASCII.GetBytes(_Configuration["Credentials:Jira"]);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Credentials));
            var jsonInString = JsonConvert.SerializeObject(issue);
            var stringcontent = new StringContent(jsonInString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_Configuration["Url:Base"] + "/jira/rest/api/2/issue", stringcontent);
            var response_string_json = response.Content.ReadAsStringAsync().Result;
            var respone_json = JsonConvert.DeserializeObject<IssueCreateResponse>(response_string_json);  

            if(respone_json.key != null)
            {
                _logger.LogInformation($"Utworzono zgłoszenie: {respone_json.key}");
            }
            else
            {
                if(respone_json.errors != null)
                {
                    _logger.LogError($"ERROR: {respone_json.errors}");
                }
                foreach(string error in respone_json.errorMessages)
                {
                     _logger.LogError($"ERROR: {error}");
                }
            }
            return respone_json;
        }

    }
}