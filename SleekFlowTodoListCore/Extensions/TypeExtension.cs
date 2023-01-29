using SleekFlowTodoListCore.Domain.Database.Todos;

namespace SleekFlowTodoListCore.Extensions
{
    public static class TypeExtension
    {
        // Ref: https://stackoverflow.com/questions/972307/how-to-loop-through-all-enum-values-in-c
        // https://stackoverflow.com/questions/67401524/get-enum-constant-values-as-list-of-integers
        public static List<int> GetEnumDataTypeValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<int>().ToList();
        }

        // Ref: https://stackoverflow.com/questions/14971631/convert-an-enum-to-liststring
        public static List<string> GetEnumDataTypes<T>()
        {
            return Enum.GetNames(typeof(T)).ToList();
        }

        // Ref: https://stackoverflow.com/questions/5583717/enum-to-dictionaryint-string-in-c-sharp
        public static Dictionary<int, string> GetEnumDataTypeDict<T>()
        {
            var paylood = new Dictionary<int, string>();

            foreach (var foo in Enum.GetValues(typeof(T)))
            {
                paylood.Add((int)foo, foo.ToString());
            }

            return paylood;
        }
    }
}
