using System.Collections.Generic;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class RAMLDocument : IDocument
    {
        public string RAMLVersion { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }
        public string BaseUri { get; set; }

        //public List<string> Protocols { get; set; }
        //public string MediaType { get; set; }
        //public List<IDictionary<string, string>> Schemas { get; set; }
        //public IDictionary<string, Parameter> BaseUriParameters { get; set; }
        //public IEnumerable<DocumentationItem> Documentation { get; set; }
        public ICollection<Resource> Resources { get; set; }

        //public IEnumerable<IDictionary<string, ResourceType>> ResourceTypes { get; set; }
        //public IEnumerable<IDictionary<string, Trait>> Traits { get; set; }
        //public IEnumerable<IDictionary<string, SecurityScheme>> SecuritySchemes { get; set; }

        public string Serialize()
        {
            return null;
        }
    }
}