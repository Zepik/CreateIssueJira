namespace CreateIssueJira.Model
{
        public class Project
        {
            public string id { get; set; }
        }

        public class Issuetype
        {
            public string name { get; set; }      
        }
        public class Reporter
        {
            public string name { get; set; }
        }
        public class Fields
        {
            public Project project { get; set; }
            public string summary { get; set; }
            public string description { get; set; }
            public Issuetype issuetype { get; set; }
            public Reporter reporter { get; set; }
        }

        public class JSON_issue
        {
            Project project = new Project();
            Issuetype isseType = new Issuetype();
            Reporter reporter = new Reporter();
            public Fields fields = new Fields();
        
            public JSON_issue(string projectId, string summary, string description, string issueTypeName, string reporterName)
            {
                project.id = projectId;
                isseType.name = issueTypeName;
                reporter.name = reporterName;

                fields.project = project;
                fields.summary = summary;
                fields.description = description;
                fields.issuetype = isseType;
                fields.reporter = reporter;

            }
        }
    
}