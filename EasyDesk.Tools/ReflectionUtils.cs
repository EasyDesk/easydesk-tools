using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyDesk.Core
{
    public static class ReflectionUtils
    {
        public static IEnumerable<T> InstancesOfSubtypesOf<T>(params Type[] assemblyTypes) =>
            InstancesOfSubtypesOf<T>(assemblyTypes.AsEnumerable());

        public static IEnumerable<Type> InstantiableSubtypesOf<T>(params Type[] assemblyTypes) =>
            InstantiableSubtypesOf<T>(assemblyTypes.AsEnumerable());

        public static IEnumerable<T> InstancesOfSubtypesOf<T>(IEnumerable<Type> assemblyTypes) =>
            InstancesOfSubtypes(typeof(T), assemblyTypes).Cast<T>();

        public static IEnumerable<Type> InstantiableSubtypesOf<T>(IEnumerable<Type> assemblyTypes) =>
            InstantiableSubtypes(typeof(T), assemblyTypes);

        public static IEnumerable<Type> InstantiableSubtypes(Type baseType, params Type[] assemblyTypes) =>
            InstantiableSubtypes(baseType, assemblyTypes.AsEnumerable());

        public static IEnumerable<object> InstancesOfSubtypes(Type baseType, params Type[] assemblyTypes) =>
            InstancesOfSubtypes(baseType, assemblyTypes.AsEnumerable());

        public static IEnumerable<Type> InstantiableTypesInAssemblies(params Type[] assemblyTypes) =>
            InstantiableTypesInAssemblies(assemblyTypes.AsEnumerable());

        public static IEnumerable<Type> InstantiableTypesInAssemblies(IEnumerable<Type> assemblyTypes)
        {
            return assemblyTypes
                .Select(t => t.Assembly)
                .SelectMany(assembly => assembly.GetTypes())
                .Where(t => !(t.IsAbstract || t.IsInterface));
        }

        public static IEnumerable<Type> InstantiableSubtypes(Type baseType, IEnumerable<Type> assemblyTypes)
        {
            return InstantiableTypesInAssemblies(assemblyTypes)
                .Where(t => t.IsAssignableTo(baseType));
        }

        public static IEnumerable<object> InstancesOfSubtypes(Type baseType, IEnumerable<Type> assemblyTypes)
        {
            return InstantiableSubtypes(baseType, assemblyTypes)
                .Select(Activator.CreateInstance);
        }

        public static object GetPropertyFromPath(object source, string path)
        {
            static object GetPropertyFromPath(object source, Type currentType, IEnumerable<string> path)
            {
                var property = path.FirstOrDefault();
                if (property == null)
                {
                    return source;
                }
                var propertyInfo = currentType.GetProperty(property);
                return GetPropertyFromPath(propertyInfo.GetValue(source), propertyInfo.PropertyType, path.Skip(1));
            }
            return GetPropertyFromPath(source, source.GetType(), path.Split("."));
        }
    }
}
