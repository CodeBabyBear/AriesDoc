using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public interface IRAMLElement
    {
        string Key { get; set; }
        int Order { get; set; }
        int Depth { get; set; }
        object Value { get; set; }

        void SerializeToString(StringBuilder sb);
    }
}