using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Cheat_Launcher.Utils
{
    class Injector
    {
        public static void Inject(Process targetProcess, string dllPath)
        {
            string resourceName = "injector.exe"; // Укажите правильное пространство имен и имя ресурса

            // Читаем ресурс через Application.GetResourceStream
            Uri resourceUri = new Uri($"pack://application:,,,/{resourceName}");
            using (var stream = Application.GetResourceStream(resourceUri).Stream)
            {
                if (stream == null)
                    throw new InvalidOperationException($"Ресурс {resourceName} не найден в сборке.");

                string tempPath = Path.Combine(Path.GetTempPath(), "injector.exe");

                using (var fileStream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                string processName = targetProcess.ProcessName + ".exe";
                string arguments = $"--process-name {processName} --inject {dllPath} {dllPath}";

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = tempPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                Process.Start(startInfo);
            }
        }
    }
}
