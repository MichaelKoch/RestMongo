using System;
using System.Collections.Generic;
using Microsoft.AspNet.OData.Builder;
using System.Linq;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using RestMongo.Interfaces;
using Microsoft.OData.UriParser;
using Microsoft.AspNet.OData.Query.Validators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNet.OData.Query;

namespace RestMongo.Attributes
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
