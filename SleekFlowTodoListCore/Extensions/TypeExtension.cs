﻿namespace SleekFlowTodoListCore.Extensions
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
    }
}