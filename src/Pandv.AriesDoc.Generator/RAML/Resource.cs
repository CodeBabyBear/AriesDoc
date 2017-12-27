using System.Text;

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

        public override void SerializeToString(StringBuilder sb)
        {
            sb.Append(Key.Indent(Depth));
            sb.AppendLine(":");
            base.SerializeToString(sb);
        }
    }
}