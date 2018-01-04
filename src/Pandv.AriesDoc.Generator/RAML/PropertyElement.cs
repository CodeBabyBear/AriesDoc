using System;
using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class PropertyElement : IRAMLElement
    {
        public string Key { get; set; }
        public int Order { get; set; }
        public object Value { get; set; }
        public int Depth { get; set; }

        public void SerializeToString(StringBuilder sb)
        {
            if (Value != null && !string.IsNullOrWhiteSpace(Value.ToString()))
            {
                if (!Value.ToString().Contains(Environment.NewLine))
                    sb.AppendLine($"{Key}: {Value}".Indent(Depth));
                else
                {
                    sb.AppendLine($"{Key}: |".Indent(Depth));
                    foreach (var item in Value.ToString()
                        .Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        sb.AppendLine(item.Indent(Depth + 1));
                    }
                }
            }
        }
    }
}