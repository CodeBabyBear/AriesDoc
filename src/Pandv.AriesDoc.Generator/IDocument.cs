namespace Pandv.AriesDoc.Generator
{
    public interface IDocument
    {
        string Title { get; set; }

        string Serialize();
    }
}