using System;

namespace AC_Control
{
    public static class Functions
    {
        public static string NotNull(object obj, string retorno = "")
        {            
            if (DBNull.Value.Equals(obj) || obj is null || string.IsNullOrEmpty(obj.ToString()))
            {
                return retorno;
            }
            return obj.ToString();
        }
        public static bool IsNumeric(string input)
        {
            return int.TryParse(input, out _);
        }

        public static object IIf(bool expression, object truePart, object falsePart)
        { return expression ? truePart : falsePart; }

        public static T IIf<T>(bool expression, T truePart, T falsePart)
        { return expression ? truePart : falsePart; }
    }
}
