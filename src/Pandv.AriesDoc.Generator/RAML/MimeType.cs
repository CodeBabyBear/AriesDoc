namespace Pandv.AriesDoc.Generator.RAML
{
    public class MimeType : ArrayElement
    {
        public Schema Schema { get; } = new Schema();
        public PropertyElement Type { get; } = new PropertyElement() { Key = "type" };

        public MimeType() : base()
        {
            WithKey = true;
            AddElement(Schema);
            AddElement(Type);
        }
    }

    public class MimeTypeV10 : ArrayElement
    {
        public PropertyElement Type { get; } = new PropertyElement() { Key = "type" };

        public MimeTypeV10() : base()
        {
            WithKey = true;
            AddElement(Type);
        }
    }
}