using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace MultiQMod
{
    internal static class Utils
    {
        public static void Log(string text)
        {
            Harmony.FileLog.Log($"{DateTime.Now.ToShortTimeString()} [MultiQMod] {text}");
        }

        public static void ToScreen(string text)
        {
            ErrorMessage.AddDebug($"{DateTime.Now.ToShortTimeString()} [MultiQMod] {text}");
        }
        public static void Log(this IEnumerable<CodeInstruction> instructions)
        {
            Log($"Logging instuctions");
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                Log($"[{i}] {codes[i]}");
            }
            Log($"End of instuctions log");
        }

        public static void LogPatches(this MethodBase method,HarmonyInstance harmony)
        {
            var patches = harmony.IsPatched(method);
            if (patches == null)
            {
                Utils.Log($"Method \"{method}\" is not patched!");
                return;
            }
            Log("Logging Prefixes...");
            foreach (var patch in patches.Prefixes)
            {
                Log($"Patch {patch.index}:\n\tOwner: {patch.owner}\n\tPatched method: {patch.patch}\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter:{patch.after}");
            }
            Log("Logging Postfixes...");
            foreach (var patch in patches.Postfixes)
            {
                Log($"Patch {patch.index}:\n\tOwner: {patch.owner}\n\tPatched method: {patch.patch}\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter:{patch.after}");
            }
            Log("Loggind Transpilers...");
            foreach (var patch in patches.Transpilers)
            {
                Log($"Patch {patch.index}:\n\tOwner: {patch.owner}\n\tPatched method: {patch.patch}\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter:{patch.after}");
            }
            Log("Done!");
        }

        #region Reflector

        private const BindingFlags flags = 
            BindingFlags.Public |
            BindingFlags.NonPublic | 
            BindingFlags.Static | 
            BindingFlags.Instance | 
            BindingFlags.DeclaredOnly;

        #region Fields
        public static IEnumerable<FieldInfo> GetAllFields(this Type type)
        {
            return type?.GetFields(flags).Union(GetAllFields(type.BaseType)) ?? Enumerable.Empty<FieldInfo>();
        }
        public static void LogAllFields(this Type type)
        {
            Log("Logging all Fields");
            foreach (var info in type.GetAllFields())
            {
                Log(info);
            }
            Log("End of Fields");
        }
        public static void Log(this FieldInfo info)
        {
            Log($"Field: \"{info}\"\n{info?.Attributes}");
        }

        public static void Log(this FieldInfo info, object obj)
        {
            Log($"Field: \"{info}\". Value: \"{info?.GetValue(obj)}\"\n{info?.Attributes}");
        }
        #endregion

        #region Methods
        public static IEnumerable<MethodInfo> GetAllMethods(this Type type)
        {
            return type?.GetMethods(flags).Union(GetAllMethods(type.BaseType)) ?? Enumerable.Empty<MethodInfo>();
        }
        public static void LogAllMethods(this Type type)
        {
            Log("Logging all Methods");
            foreach (var info in type.GetAllMethods())
            {
                Log(info);
            }
            Log("End of Methods");
        }
        public static void Log(this MethodInfo info)
        {
            Log($"Method: \"{info}\"\n\"{info?.Attributes}\"");
        }

        #endregion

        #region Constructors
        public static IEnumerable<ConstructorInfo> GetAllConstructors(this Type type)
        {
            return type?.GetConstructors(flags) ?? Enumerable.Empty<ConstructorInfo>();
        }
        public static void LogAllConstructors(this Type type)
        {
            Log("Logging all Constructors");
            foreach (var info in type.GetAllConstructors())
            {
                Log(info);
            }
            Log("End of Constructors");
        }
        public static void Log(this ConstructorInfo info)
        {
            Log($"Constructor: {info}\n{info?.Attributes}");
        }
        #endregion

        #endregion

    }
}
