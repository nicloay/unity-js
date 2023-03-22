using System.IO;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

// ReSharper disable once CheckNamespace
namespace ClearScriptUnityDemo
{
    /// <summary>
    ///     Create bin/run.bat file so it's possible to start player from command line
    /// </summary>
    public class BuildPostProcessBinFolder : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            CreateBatFile(report.summary.outputPath);
        }

        private static void CreateBatFile(string exePath)
        {
            var fileName = Path.GetFileName(exePath);
            var directory = Path.GetDirectoryName(exePath);
            var targetDirectory = Path.Combine(directory!, "bin");
            var targetFilePath = Path.Combine(targetDirectory, "run.bat");
            var content = $"start \"..\" \"{fileName}\"";
            if (!Directory.Exists(targetDirectory)) Directory.CreateDirectory(targetDirectory);

            if (File.Exists(targetFilePath)) File.Delete(targetFilePath);

            File.WriteAllText(targetFilePath, content);
        }
    }
}