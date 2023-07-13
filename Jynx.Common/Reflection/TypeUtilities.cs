namespace Jynx.Common.Reflection
{
    public static class TypeUtilities
    {
        /// <summary>
        /// Returns the first Type that meets the name requirement anywhere in the AppDomain.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type? GlobalGetType(string name)
            => AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .FirstOrDefault(t => t.Name == name);
    }
}
