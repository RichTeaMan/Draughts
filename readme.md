# Draughts AI Trainer
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

## CI
|        | Windows | Linux |
| ------ | --------|-------|
| Master | [![Build status](https://ci.appveyor.com/api/projects/status/apt6gir9l7wxun49/branch/master?svg=true)](https://ci.appveyor.com/project/RichTeaMan/draughts/branch/master) | [![Build status](https://travis-ci.org/RichTeaMan/Draughts.svg?branch=master)](https://travis-ci.org/RichTeaMan/Draughts) |

## Desktop UI

The [Draughts.UI.Wpf project](https://github.com/RichTeaMan/Draughts/tree/master/Draughts.UI.Wpf) builds a WPF application for playing any combination of human and AI players.

It can be built and run on (only) Windows computers with:
```
./cake.ps1 -target=Game
```

The latest build is available from [Appveyor](https://github.com/RichTeaMan/Draughts/tree/master/Draughts.UI.Wpf).

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
