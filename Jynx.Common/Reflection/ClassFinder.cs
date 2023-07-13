namespace Jynx.Common.Reflection
{
    public static class ClassFinder
    {
        public static IEnumerable<Type> GetChildrenOf(Type parent, FilterTypes validTypes = FilterTypes.Classes, string? ns = null)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(t => {
                    if (!parent.IsAssignableFrom(t)) return false;
                    if (IsNamespaceInNamespace(ns, t.Namespace)) return false;

                    if (validTypes.HasFlag(FilterTypes.Classes) && t.IsClass && !t.IsAbstract) return true;
                    if (validTypes.HasFlag(FilterTypes.Abstracts) && t.IsAbstract) return true;
                    if (validTypes.HasFlag(FilterTypes.Interfaces) && !t.IsClass) return true;

                    return false;
                });
        }

        public static IEnumerable<Type> GetChildrenOf<T>(FilterTypes validTypes = FilterTypes.Classes, string? ns = null) => GetChildrenOf(typeof(T), validTypes, ns);

        public static IEnumerable<Type> GetWithAttribute(Type attributeType, string? ns = null)
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.IsDefined(attributeType, true));

            foreach (var t in types)
            {
                if (IsNamespaceInNamespace(ns, t.Namespace)) continue;

                yield return t;
            }
        }

        public static IEnumerable<Type> GetWithAttribute<T>(string? ns = null) where T : class => GetWithAttribute(typeof(T), ns);

        private static bool IsNamespaceInNamespace(string? namespaceNeedle, string? namespaceHaystack)
        {
            if (namespaceNeedle == null) return false;
            
            if (namespaceHaystack == null) return false;

            if (namespaceNeedle.Length > namespaceHaystack.Length) return false;

            return namespaceHaystack[..namespaceNeedle.Length] != namespaceNeedle;
        }

        [Flags]
        public enum FilterTypes : byte
        {
            Empty = 0,
            Classes = 1,
            Interfaces = 2,
            Abstracts = 4,
        }
    }
}
