using System;
using System.Reflection;

namespace RestMongo.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class IsQueryableAttribute : Attribute
    {
        public static bool IsAssignedTo(MemberInfo member)
        {
            if (member.GetCustomAttributes(typeof(IsQueryableAttribute), false).Length > 0)
            {
                return true;
            }
            return false;
        }
    }
}
