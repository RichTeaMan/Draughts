# Draughts AI Trainer
This project attempts to train an AI Draughts player.

The AI make decisions based on weights for board metrics.
These weights are honed with a genetic algorithm.

## Cake Tasks
This project uses [Cake](https://cakebuild.net)!
* cake -target=Clean
* cake -target=Build
* cake -target=Test
* cake -target=Train
* cake -target=Show-Names
* cake -target=Game

## CI
Build Status: [![Build status](https://ci.appveyor.com/api/projects/status/apt6gir9l7wxun49?svg=true)](https://ci.appveyor.com/project/RichTeaMan/draughts)

## UI

Draughts.UI.Wpf can be configured with command line flags:

```
-w ai D:\Projects\Draughts\AiOutput\ai.json -b human
````
