#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=vswhere"
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
		DirectoryPath vsLatest  = VSWhereLatest();
		FilePath msBuildPathX64 = (vsLatest==null)
								? null
								: vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/MSBuild.exe");

		MSBuild("Draughts.sln", new MSBuildSettings {
		Verbosity = Verbosity.Minimal,
		Configuration = configuration,
		PlatformTarget = PlatformTarget.MSIL,
		ToolPath = msBuildPathX64
		});
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
    DotNetCoreExecute($"./Draughts.Ai.Trainer.CLI/bin/{buildDir}/netcoreapp1.1/Draughts.Ai.Trainer.CLI.dll");
});

Task("Show-Names")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreExecute($"./NameUtility.Display/bin/{buildDir}//netcoreapp1.1/NameUtility.Display.dll");
});

Task("Game")
    .IsDependentOn("Build")
    .Does(() =>
{
    using(var process = StartAndReturnProcess($"./Draughts.UI.Wpf/bin/{buildDir}/Draughts.UI.Wpf.exe"))
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