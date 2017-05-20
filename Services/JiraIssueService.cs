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
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace CreateIssueJira.Services
{

	public class JiraIssueService
	{
		private IConfigurationRoot _Configuration;
		private readonly ILogger _logger;
		private IHostingEnvironment _env;
		public JiraIssueService(IConfigurationRoot Configuration,  ILogger<JiraIssueService> logger, IHostingEnvironment env)
		{
			_Configuration = Configuration;
			_logger = logger;
			_env = env;
		}

		public async Task<IssueCreateResponse> CreateIssueJira(JSON_issue issue)
		{   

			IssueCreateResponse respone_json;
			using(var client = new HttpClient())
			{
				var Credentials = Encoding.ASCII.GetBytes(_Configuration["Credentials:Jira"]);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Credentials));
				var jsonInString = JsonConvert.SerializeObject(issue);
				var stringcontent = new StringContent(jsonInString, Encoding.UTF8, "application/json");
				var response = await client.PostAsync(_Configuration["Url:Base"] + "/jira/rest/api/2/issue", stringcontent);
				var response_string_json = response.Content.ReadAsStringAsync().Result;
				respone_json = JsonConvert.DeserializeObject<IssueCreateResponse>(response_string_json); 
			}
			if(respone_json.key != null)
			{
				_logger.LogInformation($"Utworzono zgłoszenie: {respone_json.key}");
			}
			else
			{
				if(respone_json.errors != null)
				{
					_logger.LogError($" {respone_json.errors}");
				}
				foreach(string error in respone_json.errorMessages)
				{
					 _logger.LogError($" {error}");
				}
			}               
			return respone_json; 

		}

		public List<string> UploadAttachments(IList<IFormFile> files)
		{
			List<string> filesnames = new List<string>();
			foreach(var file in files)
			{
				filesnames.Add(file.FileName);
				var filename = ContentDispositionHeaderValue
								.Parse(file.ContentDisposition)
								.FileName
								.Trim('"');
				filename = _env.WebRootPath + $@"/zalaczniki/{filename}";
				using (FileStream fs = System.IO.File.Create(filename))
				{
				file.CopyTo(fs);
				fs.Flush();
				}
			}
            return filesnames;
		}

		public async Task<List<HttpResponseMessage>> SendAttachmentsToIssueJira(List<string> filesNames, string issueKey)
		{   
			List<HttpResponseMessage> response = new List<HttpResponseMessage>();
			using(var client = new HttpClient())
			{
				var Credentials = Encoding.ASCII.GetBytes(_Configuration["Credentials:Jira"]);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Credentials));
				client.DefaultRequestHeaders.Add("X-Atlassian-Token", "nocheck");
				foreach(var file in filesNames)
				{
					MultipartFormDataContent content = new MultipartFormDataContent();
					HttpContent fileContent = new ByteArrayContent(File.ReadAllBytes(_env.WebRootPath + $@"/zalaczniki/{file}"));
					content.Add(fileContent, "file", file);
					string url = $"http://patek-ms-7758:2990/jira/rest/api/2/issue/{issueKey}/attachments";
					response.Add(await client.PostAsync(url, content));
				}
				
			}
			return response; 
		}
}
}