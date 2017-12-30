using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class MimeType : ArrayElement
    {
        public Schema Schema { get; } = new Schema();

        public MimeType() : base()
        {
            WithKey = true;
            AddElement(Schema);
        }
    }

    public class Schema : ArrayElement
    {
        public Schema() : base()
        {
            Key = "schema";
            WithKey = true;
        }

        protected override void SerializeKey(StringBuilder sb)
        {
            sb.Append(Key.Indent(Depth));
            sb.AppendLine(": |");
        }
    }
}