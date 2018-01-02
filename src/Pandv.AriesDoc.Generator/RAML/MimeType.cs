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
}