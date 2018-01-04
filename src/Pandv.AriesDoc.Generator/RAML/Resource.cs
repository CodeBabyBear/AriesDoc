namespace Pandv.AriesDoc.Generator.RAML
{
    public class Resource : ArrayElement
    {
        //public string RelativeUri { get => Key; set => Key = value; }
        //public string DisplayName { get; set; }
        //public ICollection<Resource> Resources { get; set; }
        //public IDictionary<string, Parameter> UriParameters { get; set; }
        //public IEnumerable<Protocol> Protocols { get; set; }
        //public IEnumerable<Method> Methods { get; set; }
        public ArrayElement UriParameters { get; } = new ArrayElement() { Key = "uriParameters", WithKey = true };

        public bool HasUriParameters { get => UriParameters.HasElements; }

        public Resource() : base()
        {
            WithKey = true;
            AddElement(UriParameters);
        }
    }
}