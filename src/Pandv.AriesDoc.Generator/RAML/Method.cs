namespace Pandv.AriesDoc.Generator.RAML
{
    public class Method : ArrayElement
    {
        public ArrayElement QueryParameters { get; } = new ArrayElement() { Key = "queryParameters", WithKey = true };
        public ArrayElement Body { get; } = new ArrayElement() { Key = "body", WithKey = true };
        public ArrayElement Responses { get; } = new ArrayElement() { Key = "responses", WithKey = true };

        public Method() : base()
        {
            WithKey = true;
            AddElement(QueryParameters);
            AddElement(Body);
            AddElement(Responses);
        }
    }
}