using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pandv.AriesDoc.Generator.RAML
{
    public class ArrayElement : IRAMLElement
    {
        private int depth;
        protected IDictionary<string, IRAMLElement> elements = new Dictionary<string, IRAMLElement>(StringComparer.OrdinalIgnoreCase);

        public string Key { get; set; }
        public int Order { get; set; }
        public object Value { get => elements; set => value.ToString(); }
        public int Count { get => elements.Count; }
        public bool WithKey { get; set; }
        public bool HasElements { get => elements.Count > 0; }

        public int Depth
        {
            get => depth;
            set
            {
                depth = value;
                foreach (var item in elements)
                {
                    item.Value.Depth = value + 1;
                }
            }
        }

        public IEnumerable<IRAMLElement> Elements { get => elements.Values.OrderBy(i => i.Order); }

        public virtual void SerializeToString(StringBuilder sb)
        {
            if (!HasElements) return;
            if (WithKey)
            {
                sb.Append(Key.Indent(Depth));
                sb.AppendLine(":");
            }
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