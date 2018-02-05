namespace Pandv.AriesDoc.Generator.RAML
{
    public class Response : ArrayElement
    {
        public ArrayElement Body { get; } = new ArrayElement() { Key = "body", WithKey = true };
        public PropertyElement Description { get; } = new PropertyElement() { Key = "description" };

        public Response()
        {
            WithKey = true;
            AddElement(Description);
            AddElement(Body);
        }
    }
}