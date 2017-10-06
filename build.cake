#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var buildDir = Directory(configuration);

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Task("Clean")
    .Does(() =>
{
    CleanDirectories("./**/bin");
});

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    NuGetRestore("./Draughts.sln");
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
      // Use MSBuild
      MSBuild("Draughts.sln", settings =>
        settings.SetConfiguration(configuration));
    }
});

Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    NUnit3($"./**/bin/{buildDir}/*.Tests.dll", new NUnit3Settings {
        NoResults = true
    });
});

Task("Train")
    .IsDependentOn("Build")
    .Does(() =>
{
    using(var process = StartAndReturnProcess($"./Draughts.Ai.Trainer/bin/{buildDir}/Draughts.Ai.Trainer.exe"))
    {
        process.WaitForExit();
        // This should output 0 as valid arguments supplied
        Information("Exit code: {0}", process.GetExitCode());
    }
});

Task("Show-Names")
    .IsDependentOn("Build")
    .Does(() =>
{
    using(var process = StartAndReturnProcess($"./NameUtility.Display/bin/{buildDir}/NameUtility.Display.exe"))
    {
        process.WaitForExit();
        Information("Exit code: {0}", process.GetExitCode());
    }
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Run-Unit-Tests");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);