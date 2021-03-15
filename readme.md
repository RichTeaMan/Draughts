# Draughts AI Trainer

[![Draughts build](https://github.com/RichTeaMan/Draughts/actions/workflows/build.yaml/badge.svg)](https://github.com/RichTeaMan/Draughts/actions/workflows/build.yaml)

This project attempts to train an AI Draughts player.

The AI make decisions based on weights for board metrics.
These weights are honed with a genetic algorithm.

A playable version can be found at https://draughts.richteaman.com.

## Cake Tasks
This project uses [Cake](https://cakebuild.net)!
* cake -target=Clean
* cake -target=Build
* cake -target=Test
* cake -target=Train
* cake -target=Show-Names
* cake -target=Game
* cake -target=Web-UI

## Desktop UI

The [Draughts.UI.Wpf project](https://github.com/RichTeaMan/Draughts/tree/master/Draughts.UI.Wpf) builds a WPF application for playing any combination of human and AI players.

It can be built and run on (only) Windows computers with:
```
./cake.ps1 -target=Game
```

The latest build is available from [Appveyor](https://ci.appveyor.com/api/projects/RichTeaMan/Draughts/artifacts/Draughts.UI.Wpf%2Fbin%2FRelease%2FDraughtsUI.zip).

Draughts.UI.Wpf can be configured with command line flags:

```
-w ai D:\Projects\Draughts\AiOutput\ai.json -b human
````

## Web UI

The [Draughts.Web.UI project](https://github.com/RichTeaMan/Draughts/tree/master/Web/Draughts.Web.UI) builds an ASP.NET Core project builds a web application for a human to player an AI opponent.

It can be built and run on [most operating systems](https://github.com/dotnet/core/blob/master/release-notes/2.2/2.2-supported-os.md) using:

```
./cake.ps1 -target=Web-UI
```

A live version is available at https://draughts.richteaman.com.
