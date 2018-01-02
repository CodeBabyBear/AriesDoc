namespace Pandv.AriesDoc.Generator
{
    public interface IDocument
    {
        string Title { get; set; }

        string BaseUri { get; set; }

        string Serialize();
    }
}