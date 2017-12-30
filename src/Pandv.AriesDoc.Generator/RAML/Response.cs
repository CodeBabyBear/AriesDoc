namespace Pandv.AriesDoc.Generator.RAML
{
    public class Response : ArrayElement
    {
        public ArrayElement Body { get; } = new ArrayElement() { Key = "body", WithKey = true };

        public Response()
        {
            WithKey = true;
            AddElement(Body);
        }
    }
}