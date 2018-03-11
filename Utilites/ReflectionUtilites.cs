using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Utilites;
namespace Utilites
{
    internal static class ReflectionUtilites
    {

        private const BindingFlags Flags =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static |
            BindingFlags.Instance |
            BindingFlags.DeclaredOnly;

        #region Fields

        public static IEnumerable<FieldInfo> GetAllFields(this Type type) =>
            type?.GetFields(Flags).Union(GetAllFields(type.BaseType)) ?? Enumerable.Empty<FieldInfo>();

        public static void LogAllFields(this Type type)
        {
            Logger.Debug("Logging all Fields");
            foreach (var info in type.GetAllFields())
                Log(info);
            Logger.Debug("End of Fields");
        }

        public static void Log(this FieldInfo info) =>
            Logger.Debug($"Field: \"{info}\"\n{info?.Attributes}");

        public static void Log(this FieldInfo info, object obj) =>
            Logger.Debug($"Field: \"{info}\". Value: \"{info?.GetValue(obj)}\"\n{info?.Attributes}");
        #endregion

        #region Methods

        public static IEnumerable<MethodInfo> GetAllMethods(this Type type) =>
            type?.GetMethods(Flags).Union(GetAllMethods(type.BaseType)) ?? Enumerable.Empty<MethodInfo>();

        public static void LogAllMethods(this Type type)
        {
            Logger.Debug("Logging all Methods");
            foreach (var info in type.GetAllMethods())
                Log(info);
            Logger.Debug("End of Methods");
        }

        public static void Log(this MethodInfo info) =>
            Logger.Debug($"Method: \"{info}\"\n\"{info?.Attributes}\"");

        #endregion

        #region Constructors

        public static IEnumerable<ConstructorInfo> GetAllConstructors(this Type type) =>
            type?.GetConstructors(Flags) ?? Enumerable.Empty<ConstructorInfo>();

        public static void LogAllConstructors(this Type type)
        {
            Logger.Debug("Logging all Constructors");
            foreach (var info in type.GetAllConstructors())
                Log(info);
            Logger.Debug("End of Constructors");
        }

        public static void Log(this ConstructorInfo info)=>
            Logger.Debug($"Constructor: {info}\n{info?.Attributes}");
        #endregion
    }
}
