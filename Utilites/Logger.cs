using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Harmony;

namespace Utilites
{
    #region Log levels and types.
    public enum LogLevel
    {
        /// <summary>
        /// Just an information message.
        /// </summary>
        Info,
        /// <summary>
        /// Something goes wrong, but we can handle it.
        /// </summary>
        Warning,
        /// <summary>
        /// The error has occur.
        /// </summary>
        Error,
        /// <summary>
        /// Debug messages that should be removed in the relized versions.
        /// </summary>
        Debug
    }

    public enum LogType
    {
        /// <summary>
        /// Print the information to the file "harmony.log.txt" located at the desktop.
        /// </summary>
        Harmony,
        /// <summary>
        /// Print the information to the "output_log.txt".
        /// </summary>
        Console,
        /// <summary>
        /// Print the information on the player screen
        /// </summary>
        PlayerScreen
    }
    #endregion

    public static class Logger
    {
        public static void Debug(string text, LogType type = LogType.Harmony) =>
            Log(text, LogLevel.Debug, type);

        public static void Error(string text, LogType type = LogType.Harmony) =>
            Log(text, LogLevel.Error, type);

        public static void Info(string text, LogType type = LogType.Harmony) =>
            Log(text, LogLevel.Info, type);

        public static void Warning(string text, LogType type = LogType.Harmony) =>
            Log(text, LogLevel.Warning, type);

        private static void Log(string text, LogLevel level, LogType type)
        {
            var caller = Assembly.GetCallingAssembly().GetName().Name;
            switch (type)
            {
                case LogType.Harmony:
                    Harmony.FileLog.Log($"{DateTime.Now.ToShortTimeString()} [{caller}] [{level}] {text}");
                    return;
                case LogType.Console:
                    Console.WriteLine($"[{caller}] [{level}] {text}]");
                    return;
                case LogType.PlayerScreen:
                    ErrorMessage.AddDebug($"{DateTime.Now.ToShortTimeString()} [{caller}] [{level}] {text}");
                    return;
            }
        }

        #region Exceptions
        public static void Log(this Exception e, LogType logtype = LogType.Harmony) =>
            Debug(FormatException(e), logtype);

        private static string FormatException(Exception e)
        {
            if (e == null)
                return string.Empty;
            return $"\"Exception: {e.GetType()}\"\n\tMessage: {e.Message}\n\tStacktrace: {e.StackTrace}\n" +
                   FormatException(e.InnerException);
        }
        #endregion

        #region Harmony
        public static void Log(this IEnumerable<CodeInstruction> instructions, LogType logType = LogType.Harmony)
        {
            Debug($"Logging instuctions", logType);
            var codes = new List<CodeInstruction>(instructions);
            for (int i = 0; i < codes.Count; i++)
            {
                Debug($"[{i}] {codes[i]}", logType);
            }
            Debug($"End of instuctions log", logType);
        }

        public static void LogPatches(this MethodBase method, HarmonyInstance harmony, LogType logType = LogType.Harmony)
        {
            var patches = harmony.IsPatched(method);
            if (patches == null)
            {
                Debug($"Method \"{method}\" is not patched!", logType);
                return;
            }
            Debug("Logging Prefixes...", logType);
            foreach (var patch in patches.Prefixes)
            {
                Debug($"Patch {patch.index}:\n\tOwner: {patch.owner}\n\tPatched method: {patch.patch}\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter:{patch.after}", logType);
            }
            Debug("Logging Postfixes...", logType);
            foreach (var patch in patches.Postfixes)
            {
                Debug($"Patch {patch.index}:\n\tOwner: {patch.owner}\n\tPatched method: {patch.patch}\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter:{patch.after}", logType);
            }
            Debug("Loggind Transpilers...", logType);
            foreach (var patch in patches.Transpilers)
            {
                Debug($"Patch {patch.index}:\n\tOwner: {patch.owner}\n\tPatched method: {patch.patch}\n\tPriority: {patch.priority}\n\tBefore: {patch.before}\n\tAfter:{patch.after}", logType);
            }
            Debug("Done!", logType);
        }
        #endregion
    }
}
