namespace Pandv.AriesDoc
{
    public class DocConfig
    {
        public string RamlVersion { get; set; }
        public string StartupClassName { get; set; }
        public string BaseUrl { get; set; }
        public string DocDirectory { get; set; }
        public string PublishDllDirectory { get; set; }
        public bool IsRelativePath { get; set; }
    }
}