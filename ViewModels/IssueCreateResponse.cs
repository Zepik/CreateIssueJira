namespace CreateIssueJira.ViewModels
{
    public class IssueCreateResponse
    {
        public string id { get; set; }
        public string key { get; set; }
        public string self { get; set; }
        public string[] errorMessages { get; set; }
        public object errors { get; set; }
    }
}