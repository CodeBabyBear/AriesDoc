namespace Pandv.AriesDoc.Generator.RAML
{
    public class Method : ArrayElement
    {
        public ArrayElement QueryParameters { get; } = new ArrayElement() { Key = "queryParameters", WithKey = true };
        public ArrayElement Body { get; } = new ArrayElement() { Key = "body", WithKey = true };
        public ArrayElement Responses { get; } = new ArrayElement() { Key = "responses", WithKey = true };
        public PropertyElement Description { get; } = new PropertyElement() { Key = "description" };

        public Method() : base()
        {
            WithKey = true;
            AddElement(Description);
            AddElement(QueryParameters);
            AddElement(Body);
            AddElement(Responses);
        }
    }
}