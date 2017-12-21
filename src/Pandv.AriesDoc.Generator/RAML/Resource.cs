using System.Collections.Generic;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class Resource : BasicInfo
    {
        public string RelativeUri { get; set; }
        public string DisplayName { get; set; }
        public ICollection<Resource> Resources { get; set; }
        public IDictionary<string, Parameter> UriParameters { get; set; }
        public IEnumerable<Protocol> Protocols { get; set; }
        public IEnumerable<Method> Methods { get; set; }
    }
}