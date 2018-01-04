using System;
using System.Collections.Generic;
using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ObjectType : ArrayElement
    {
        private PropertyElement type = new PropertyElement() { Key = "type" };
        public string Type { get => type.Value.ToString(); set => type.Value = value.ToString(); }

        public ArrayElement Properties { get; } = new ArrayElement() { Key = "properties", WithKey = true };

        public ArrayElement Enum { get; } = new ArrayElement() { Key = "enum", WithKey = true };

        //public int? MinProperties { get; set; }

        //public int? MaxProperties { get; set; }

        //public object AdditionalProperties { get; set; }

        //public object PatternProperties { get; set; }

        //public object Discriminator { get; set; }

        //public string DiscriminatorValue { get; set; }

        public ObjectType()
        {
            WithKey = true;
            AddElement(type);
            AddElement(Properties);
            AddElement(Enum);
        }

        public void AddPropertyType(PropertyType type)
        {
            Properties.AddElement(type);
        }
    }

    public class PropertyType : ArrayElement
    {
        private PropertyElement required = new PropertyElement() { Key = "required" };
        public bool Required { get => bool.Parse(required.Value.ToString()); set => required.Value = value.ToString().ToLower(); }
        private PropertyElement type = new PropertyElement() { Key = "type" };
        public string Type { get => type.Value.ToString(); set => type.Value = value.ToString(); }

        public PropertyType()
        {
            WithKey = true;
            AddElement(required);
            AddElement(type);
        }
    }

    public class ArrayType : ArrayElement
    {
        //public bool? UniqueItems { get; set; }

        //public int? MinItems { get; set; }

        //public int? MaxItems { get; set; }

        private PropertyElement ItemType = new PropertyElement() { Key = "type" };

        public ArrayType()
        {
            AddElement(ItemType);
        }
    }
}
