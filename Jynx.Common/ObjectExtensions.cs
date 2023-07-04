using System.Dynamic;

namespace Jynx.Common
{
    public static class ObjectExtensions
    {
        public static dynamic ToDynamic(this object obj)
        {
            IDictionary<string, object?> eObj = new ExpandoObject()!;

            var objProps = obj.GetType().GetProperties();

            foreach (var prop in objProps)
            {
                eObj[prop.Name] = prop.GetValue(obj);
            }

            return eObj;
        }
    }
}
