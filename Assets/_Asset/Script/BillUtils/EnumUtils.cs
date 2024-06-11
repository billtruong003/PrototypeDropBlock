using System;
using UnityEngine;

namespace BillUtils.EnumUtilities
{
    public static class EnumUtils
    {
        public static T GetEnumValueAtIndex<T>(int index) where T : Enum
        {
            T[] values = (T[])Enum.GetValues(typeof(T));
            if (index >= 0 && index < values.Length)
            {
                return values[index];
            }
            throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} is out of range for enum {typeof(T).Name}");
        }

        public static int GetEnumCount<T>() where T : Enum
        {
            return Enum.GetValues(typeof(T)).Length;
        }
    }
}