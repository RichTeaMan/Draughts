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
var contestantFile = Argument<string>("contestant-file", null);
var outputFile = Argument<string>("output-path", null);

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
    try {
        DeleteFiles("./**/bin/**/**/**");
        DeleteFiles("./**/bin/**/**");
        CleanDirectories("./**/bin/**/**");
        CleanDirectories("./**/bin/**");
    } catch (Exception ex) {
        Information($"Failed to clean: {ex}");
    }
});

Task("Restore-NuGet-Packages")
    .Does(() =>
{
    if(IsRunningOnWindows())
    {
        NuGetRestore("./Draughts.sln");
    }
    else
    {
        DotNetCoreRestore("./Draughts.sln");
    }
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

        // Zip WPF
        var dllFiles = GetFiles("./Draughts.UI.Wpf/bin/Release/*.dll");
        var exeFiles = GetFiles("./Draughts.UI.Wpf/bin/Release/*.exe");
        Zip("./Draughts.UI.Wpf/bin/Release", "./Draughts.UI.Wpf/bin/Release/DraughtsUI.zip", dllFiles.Concat(exeFiles));
    }
    else {
        DotNetCoreBuild("./Draughts.Ai.Trainer.CLI/Draughts.Ai.Trainer.CLI.csproj", new DotNetCoreBuildSettings {
            Verbosity = DotNetCoreVerbosity.Minimal,
            Configuration = configuration
        });

        DotNetCoreBuild("./NameUtility.Display/NameUtility.Display.csproj", new DotNetCoreBuildSettings {
            Verbosity = DotNetCoreVerbosity.Minimal,
            Configuration = configuration
        });
    }

    var publishSettings = new DotNetCorePublishSettings
    {
        Configuration = configuration
    };
    DotNetCorePublish("./Web/Draughts.Web.UI/Draughts.Web.UI.csproj", publishSettings);
    DotNetCorePublish("./Draughts.Ai.Trainer.CLI/Draughts.Ai.Trainer.CLI.csproj", publishSettings);
});

Task("Test")
    .IsDependentOn("Clean")
    .IsDependentOn("Build")
    .Does(() =>
{

    DotNetCoreTest("./Draughts.Service.Tests/Draughts.Service.Tests.csproj");
    DotNetCoreTest("./NameUtility.Tests/NameUtility.Tests.csproj");
    DotNetCoreTest("./Web/Draughts.Web.UI.Mapper.Tests/Draughts.Web.UI.Mapper.Tests.csproj");

    if (IsRunningOnWindows()) {
        DirectoryPath vsLatest  = VSWhereLatest();
        FilePath vsTestPathX64 = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./Common7/IDE/Extensions/TestPlatform/vstest.console.exe");

        VSTest($"./**/bin/{buildDir}/*.Tests.dll", new VSTestSettings {
            ToolPath = vsTestPathX64
        });
    }
});

Task("Train")
    .IsDependentOn("Build")
    .Does(() =>
{
    var command = $"train -generation-count {generationCount} -iteration-count {iterationCount} -ai-type NeuralNet";
    if (null != threadCount) {
        command += $" -threads {threadCount}";
    }
    if (null != seed) {
        command += $" -seed {seed}";
    }
    if (null != contestantFile) {
        command += $" -contestant-file {contestantFile}";
    }
    if (null != outputFile) {
        command += $" -output-path {outputFile}";
    }

    DotNetCoreExecute($"./Draughts.Ai.Trainer.CLI/bin/{buildDir}/netcoreapp2.1/Draughts.Ai.Trainer.CLI.dll",
         command
    );
});

Task("Web-UI")
    .IsDependentOn("Build")
    .Does(() =>
{
    var publishDirectory = $"./Web/Draughts.Web.UI/bin/{buildDir}/netcoreapp2.1/publish";
    var executeSettings = new DotNetCoreExecuteSettings
    {
        WorkingDirectory = publishDirectory
    };
    DotNetCoreExecute($"{publishDirectory}/Draughts.Web.UI.dll", $"-- {contestantFile}", executeSettings);
});

Task("Show-Names")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreExecute($"./NameUtility.Display/bin/{buildDir}/netcoreapp2.0/NameUtility.Display.dll");
});

Task("Game")
    .IsDependentOn("Build")
    .Does(() =>
{
    if (IsRunningOnWindows()) {
        using(var process = StartAndReturnProcess($"./Draughts.UI.Wpf/bin/{buildDir}/Draughts.UI.Wpf.exe"))
        {
            process.WaitForExit();
            Information("Exit code: {0}", process.GetExitCode());
        }
    }
    else
    {
        throw new Exception("Cannot run game UI on non Windows OS.");
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
