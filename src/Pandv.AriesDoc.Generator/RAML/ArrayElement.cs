using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ArrayElement : IRAMLElement
    {
        protected IDictionary<string, IRAMLElement> elements = new Dictionary<string, IRAMLElement>(StringComparer.OrdinalIgnoreCase);

        public string Key { get; set; }
        public int Order { get; set; }
        public object Value { get => elements; set => value.ToString(); }
        public int Depth { get; set; }
        public IEnumerable<IRAMLElement> Elements { get => elements.Values.OrderBy(i => i.Order); }

        public virtual void SerializeToString(StringBuilder sb)
        {
            foreach (var item in Elements)
            {
                item.SerializeToString(sb);
            }
        }

        public virtual void AddElement(IRAMLElement element)
        {
            element.Order = Order++;
            element.Depth = Depth + 1;
            elements.Add(element.Key, element);
        }

        public T TryGetElement<T>(string key) where T : class, IRAMLElement
        {
            return elements.ContainsKey(key) ? elements[key] as T : null;
        }
    }
}