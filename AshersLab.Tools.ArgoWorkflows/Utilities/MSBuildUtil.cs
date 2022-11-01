using System.Diagnostics;
using System.Text.RegularExpressions;

namespace AshersLab.Tools.ArgoWorkflows.Utilities;

public static class MSBuildUtil
{
    public static void SetMsBuildPath()
    {
        try
        {
            ProcessStartInfo startInfo = new("dotnet", "--list-sdks")
            {
                RedirectStandardOutput = true
            };

            Process process = Process.Start(startInfo)!;
            process.WaitForExit(1000);

            string output = process.StandardOutput.ReadToEnd();
            IEnumerable<string> sdkPaths = Regex.Matches(output, "([0-9]+.[0-9]+.[0-9]+) \\[(.*)\\]")
                .Select(m => Path.Combine(m.Groups[2].Value, m.Groups[1].Value, "MSBuild.dll"));

            string sdkPath = sdkPaths.Last();
            Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", sdkPath);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Could not set MSBUILD_EXE_PATH: " + exception);
        }
    }
}