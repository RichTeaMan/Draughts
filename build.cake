#tool nuget:?package=NUnit.ConsoleRunner&version=3.4.0
#tool "nuget:?package=vswhere"
//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

var generationCount = Argument("generation-count", 20);
var iterationCount = Argument("iteration-count", 100);
var threadCount = Argument<int?>("threads", null);
var seed = Argument<int?>("seed", null);

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
    CleanDirectories("./**/bin/**");
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

        Information($"MS Build Path: {msBuildPathX64}");
        MSBuild("Draughts.sln", new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        Configuration = configuration,
        PlatformTarget = PlatformTarget.MSIL,
        ToolPath = msBuildPathX64
        });
    }
});

Task("Test")
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
    var command = $"train -generation-count {generationCount} -iteration-count {iterationCount}";
    if (null != threadCount) {
        command += $" -threads {threadCount}";
    }
    if (null != seed) {
        command += $" -seed {seed}";
    }
    DotNetCoreExecute($"./Draughts.Ai.Trainer.CLI/bin/{buildDir}/netcoreapp1.1/Draughts.Ai.Trainer.CLI.dll",
         command
    );
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
    .IsDependentOn("Test");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);