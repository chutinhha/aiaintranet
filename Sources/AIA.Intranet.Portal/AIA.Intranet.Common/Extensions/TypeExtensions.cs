﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace AIA.Intranet.Common.Extensions
{
    public static class TypeExtensions
    {
        private class SimpleTypeComparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y)
            {
                var result = x.Name == "T" || ( x.Assembly == y.Assembly &&
                    x.Namespace == y.Namespace &&
                     x.Name == y.Name);

                return result;
            }

            public int GetHashCode(Type obj)
            {
                throw new NotImplementedException();
            }
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] parameterTypes)
        {
            var methods = type.GetMethods();
            foreach (var method in methods.Where(m => m.Name == name))
            {
                var methodParameterTypes = method.GetParameters().Select(p => p.ParameterType).ToArray();

                if (methodParameterTypes.SequenceEqual(parameterTypes, new SimpleTypeComparer()))
                {
                    return method;
                }
            }

            return null;
        }
    }
}
