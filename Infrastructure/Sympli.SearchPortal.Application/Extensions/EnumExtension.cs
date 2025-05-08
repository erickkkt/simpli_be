
using System.Reflection;

namespace Sympli.SearchPortal.Application.Extensions
{
    public static class EnumExtension
    {
        public static string GetDescription(this Enum genericEnum)
        {
            Type genericEnumType = genericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_attribs != null && _attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_attribs.ElementAt(0)).Description;
                }
            }
            return genericEnum.ToString();
        }

        public static List<T> GetItems<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
            {
                throw new ArgumentException("T must be an enum type.");
            }

            List<T> list = new List<T>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                list.Add(value);
            }

            return list;
        }

        public static T GetEnumValue<T>(this string str) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }
            T val;
            return Enum.TryParse<T>(str, true, out val) ? val : default(T);
        }

        public static T GetEnumValue<T>(this int intValue) where T : struct, IConvertible
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new Exception("T must be an Enumeration type.");
            }

            return (T)Enum.ToObject(enumType, intValue);
        }
    }
}
