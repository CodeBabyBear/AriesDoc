using System;
using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class RAMLDocument : ArrayElement, IDocument
    {
        public const string KeyRAMLVersion = "#%RAML ";
        public const string RAMLVersion08 = "0.8";
        public const string RAMLVersion10 = "1.0";
        public const string KeyTitle = "title";
        public const string KeyBaseUri = "baseUri";
        public const string KeyVersion = "version";
        public const string KeyMediaType = "mediaType";

        public string RAMLVersion { get => elements[KeyRAMLVersion].Value.ToString(); set => elements[KeyRAMLVersion].Value = value; }
        public string Title { get => elements[KeyTitle].Value?.ToString(); set => elements[KeyTitle].Value = value; }
        public string Version { get => elements[KeyVersion].Value.ToString(); set => elements[KeyVersion].Value = value; }
        public string BaseUri { get => elements[KeyBaseUri].Value.ToString(); set => elements[KeyBaseUri].Value = value; }

        //public List<string> Protocols { get; set; }
        //public string MediaType { get; set; }
        //public List<IDictionary<string, string>> Schemas { get; set; }
        //public IDictionary<string, Parameter> BaseUriParameters { get; set; }
        //public IEnumerable<DocumentationItem> Documentation { get; set; }
        public ArrayElement Types { get; private set; } = new ArrayElement() { Key = "types", WithKey = true };

        public ArrayElement Resources { get; private set; } = new ArrayElement() { Key = nameof(Resources) };

        //public IEnumerable<IDictionary<string, ResourceType>> ResourceTypes { get; set; }
        //public IEnumerable<IDictionary<string, Trait>> Traits { get; set; }
        //public IEnumerable<IDictionary<string, SecurityScheme>> SecuritySchemes { get; set; }

        public RAMLDocument()
        {
            Depth = -1;
            AddElement(new StringElement() { Key = KeyRAMLVersion, Value = RAMLVersion08 });
            AddElement(new PropertyElement() { Key = KeyTitle, Value = string.Empty });
            AddElement(new PropertyElement() { Key = KeyBaseUri, Value = string.Empty });
            AddElement(new PropertyElement() { Key = KeyVersion, Value = string.Empty });
            AddElement(Types);
            AddElement(Resources);
            Resources.Depth = Depth;
        }

        public Resource GetOrAddResource(string key, Func<Resource> func = null)
        {
            var resource = Resources.TryGetElement<Resource>(key);
            if (resource == null)
            {
                resource = func == null ? new Resource() { Key = key } : func();
                Resources.AddElement(resource);
            }
            return resource;
        }

        public string Serialize()
        {
            var sb = new StringBuilder();
            SerializeToString(sb);
            return sb.ToString();
        }
    }
}