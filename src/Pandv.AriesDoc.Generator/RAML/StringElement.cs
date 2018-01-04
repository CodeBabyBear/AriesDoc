using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class StringElement : IRAMLElement
    {
        public string Key { get; set; }
        public int Order { get; set; }
        public object Value { get; set; }
        public int Depth { get; set; }

        public void SerializeToString(StringBuilder sb)
        {
            if (Value != null)
            {
                sb.AppendLine($"{Key}{Value}".Indent(Depth));
            }
        }
    }
}