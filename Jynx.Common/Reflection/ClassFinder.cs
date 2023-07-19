using System.Reflection;

namespace Jynx.Common.Reflection
{
    public static class ClassFinder
    {
        public static IEnumerable<Type> GetChildrenOf(Type parentType, FilterTypes validTypes = FilterTypes.Classes, string? ns = null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var returnTypes = new List<Type>();

            foreach(var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetLoadedTypes();

                foreach (var assemblyType in assemblyTypes)
                {
                    if (TypeMeetsRequirements(assemblyType, parentType, validTypes, ns))
                        continue;

                    returnTypes.Add(assemblyType);
                }
            }

            return returnTypes;
        }

        public static IEnumerable<Type> GetChildrenOf<T>(FilterTypes validTypes = FilterTypes.Classes, string? ns = null)
            => GetChildrenOf(typeof(T), validTypes, ns);

        public static IEnumerable<Type> GetWithAttribute(Type attributeType, string? ns = null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var returnTypes = new List<Type>();

            foreach (var assembly in assemblies)
            {
                var assemblyTypes = assembly.GetLoadedTypes();

                foreach (var assemblyType in assemblyTypes)
                {
                    if (IsNamespaceInNamespace(ns, assemblyType.Namespace))
                        continue;

                    if (!assemblyType.IsDefined(attributeType, true))
                        continue;

                    returnTypes.Add(assemblyType);
                }
            }

            return returnTypes;
        }

        public static IEnumerable<Type> GetWithAttribute<T>(string? ns = null)
            => GetWithAttribute(typeof(T), ns);

        private static bool TypeMeetsRequirements(Type childType, Type parentType, FilterTypes validTypes = FilterTypes.Classes, string? ns = null)
        {
            if (!parentType.IsAssignableFrom(childType))
                return false;

            if (IsNamespaceInNamespace(ns, childType.Namespace))
                return false;

            if (childType.IsAbstract && !validTypes.HasFlag(FilterTypes.Abstracts))
                return false;

            if (childType.IsClass && !validTypes.HasFlag(FilterTypes.Classes))
                return false;
            
            if (!childType.IsClass && !validTypes.HasFlag(FilterTypes.Interfaces))
                return false;

            return true;
        }

        private static bool IsNamespaceInNamespace(string? namespaceNeedle, string? namespaceHaystack)
        {
            if (string.IsNullOrWhiteSpace(namespaceNeedle))
                return true;
            
            if (string.IsNullOrWhiteSpace(namespaceHaystack))
                return false;

            if (namespaceHaystack[..Math.Min(namespaceNeedle.Length, namespaceHaystack.Length)] != namespaceNeedle)
                return false;

            return true;
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
