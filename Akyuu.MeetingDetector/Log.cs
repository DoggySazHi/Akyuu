using Microsoft.Extensions.Logging;

namespace Akyuu.MeetingDetector;

public static class Log
{
    public static void Error<T>(this ILogger<T>? logger, string? text = null, Exception? ex = null)
    {
        if (logger != null)
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
#pragma warning disable CA2254
            logger.LogError(ex, text);
#pragma warning restore CA2254
        }
        else
        {
            Console.WriteLine(text);
        }
    }
}