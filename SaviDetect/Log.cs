//Adapted from here: https://gist.github.com/litetex/b88fe0531e5acea82df1189643fb1f79

using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;

namespace SaviDetect
{
    public static class Log
    {
        private static string FormatForContext(this string message, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0,
            Exception ex = null)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFilePath);
            return $"{fileName} [SaviDetect:{sourceLineNumber}] [{memberName}] {message} {ex}";
        }

        public static void Verbose(string message, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Serilog.Log.Verbose(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
               );
        }

        public static void Plain(string message)
        {
            Serilog.Log.Verbose(message);
        }

        public static void Verbose(string message, Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Serilog.Log.Verbose(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber, ex)
               );
        }

        public static void Debug(string message, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Serilog.Log.Debug(message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
               );
        }

        public static void Debug(string message, Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Serilog.Log.Debug(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber, ex)
               );
        }

        public static void Info(string message, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            Serilog.Log.Information(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber)
               );
        }

        public static void Info(string message, Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {

            Serilog.Log.Information(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber, ex));
        }

        public static void Warn(string message, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {

            color = color ?? Color.Yellow;
            Serilog.Log.Warning(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Warn(string message, Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Yellow;
            Serilog.Log.Warning(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber, ex));
        }

        public static void Error(string message, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Red;
            Serilog.Log.Error(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Error(string message, Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Red;
            Serilog.Log.Error(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber, ex));
        }

        public static void Error(Exception ex, Color? color = null, [CallerMemberName]
      string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Red;
            var message = (ex != null ? ex.ToString() : "");
            Serilog.Log.Error(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Fatal(string message, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Red;
            FatalAction();

            Serilog.Log.Error(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber));
        }

        public static void Fatal(string message, Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Red;
            FatalAction();

            Serilog.Log.Error(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber, ex));
        }

        public static void Fatal(Exception ex, Color? color = null, [CallerMemberName] string memberName = "",
            [CallerFilePath] string sourceFilePath = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            color = color ?? Color.Red;
            FatalAction();
            var message = (ex == null ? ex.ToString() : "");
            Serilog.Log.Error(
               message
               .FormatForContext(memberName, sourceFilePath, sourceLineNumber));
        }

        private static void FatalAction()
        {
            Environment.ExitCode = -1;
        }
    }
}