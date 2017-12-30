namespace Pandv.AriesDoc.Generator.RAML
{
    public class Parameter : ArrayElement
    {
        public const string KeyType = "type";
        public const string KeyRequired = "required";
        public const string KeyDefault = "default";
        public string ParameterType { get => elements[KeyType].Value.ToString(); set => elements[KeyType].Value = value; }
        public string Required { get => elements[KeyRequired].Value.ToString(); set => elements[KeyRequired].Value = value; }
        public string Default { get => elements[KeyDefault].Value.ToString(); set => elements[KeyDefault].Value = value; }

        public Parameter()
        {
            WithKey = true;
            AddElement(new PropertyElement() { Key = KeyType });
            AddElement(new PropertyElement() { Key = KeyRequired });
            AddElement(new PropertyElement() { Key = KeyDefault });
        }
    }
}