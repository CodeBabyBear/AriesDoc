using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class Schema : ArrayElement
    {
        public Schema() : base()
        {
            Key = "schema";
            WithKey = true;
        }

        public override void SerializeToString(StringBuilder sb)
        {
            var nsb = new StringBuilder();
            WithKey = false;
            base.SerializeToString(nsb);
            nsb.Replace("|", "");
            sb.Append(Key.Indent(Depth));
            sb.AppendLine(": |");
            sb.Append(nsb.ToString());
            WithKey = true;
        }
    }
}