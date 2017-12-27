namespace Pandv.AriesDoc.Generator.RAML
{
    public static class RAMLHelper
    {
        public static string Indent(this string str, int depth)
        {
            var indent = depth <= 0 ? string.Empty : new string(' ', depth * 2);
            return indent + str;
        }
    }
}