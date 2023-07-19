using System.Reflection;

namespace Jynx.Common.Reflection
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Type> GetLoadedTypes(this Assembly assembly)
        {
            var types = new List<Type>();

            try
            {
                types.AddRange(assembly.GetTypes());
            }
            catch (ReflectionTypeLoadException ex)
            {
                types.AddRange(ex.Types.Where(t => t is not null)!);
            }

            return types;
        }
    }
}
