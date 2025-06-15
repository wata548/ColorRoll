using System;
using System.Reflection;
using UnityEngine.Rendering;

namespace Extensions {

    public static class ExEnum {
        public static bool IsFlag<T>(T data) 
            where T: struct, IConvertible
        {
            var type = typeof(T);
            var isFlagsAttribute = type.GetCustomAttribute(typeof(FlagsAttribute)) != null;
            if (!type.IsEnum || !isFlagsAttribute) {
                throw new ArgumentException("argument should be flags enum");
                return false;
            }

            int value = data.ToInt32(null);
            return (value & (value - 1)) == 0;
        }
    }
}