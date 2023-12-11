﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aehyok.Infrastructure.TypeFinders
{
    /// <summary>
    /// 通过反射获取程序集中的类型
    /// </summary>
    public static class TypeFinders
    {
        /// <summary>
        /// 判断给定的类型是否实现了指定的接口
        /// </summary>
        /// <param name="givenType"></param>
        /// <param name="genericInterfaceType"></param>
        /// <returns></returns>
        public static bool IsAssignableToGenericInterface(Type givenType, Type genericInterfaceType)
        {
            var interfaceTypes = givenType.GetInterfaces();
            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericInterfaceType)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            return !givenType.IsInterface && !givenType.IsAbstract && givenType.IsAssignableTo(genericType);
        }

        /// <summary>
        /// 根据指定类型接口获取所有实现该接口的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Type> SearchTypes(Type type)
        {
            // 通过aehyok开头的来查找当前使用的程序中的所有程序集
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(item => item.FullName.StartsWith("aehyok.")).ToList();
            var types = new List<Type>();
            try
            {
                foreach (var assembly in assemblies)
                {
                    foreach (var handleType in assembly.GetTypes())
                    {
                        if (IsAssignableToGenericType(handleType, type))
                        {
                            types.Add(handleType);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return types;
        }
    }
}
