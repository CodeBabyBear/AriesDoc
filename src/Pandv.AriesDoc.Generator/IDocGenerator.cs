using System.Collections.Generic;

namespace Pandv.AriesDoc.Generator
{
    public interface IDocGenerator
    {
        IEnumerable<IDocument> Generate();
    }
}