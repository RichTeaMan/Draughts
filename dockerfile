FROM mcr.microsoft.com/dotnet/aspnet:3.1

LABEL org.opencontainers.image.source https://github.com/RichTeaMan/Draughts

COPY Draughts.Ai.Trainer.CLI/bin/Release/netcoreapp3.1/publish/ /Draughts/
WORKDIR /Draughts
ENTRYPOINT dotnet Draughts.Ai.Trainer.CLI.dll -- train -generation-count 100 -iteration-count 20 -ai-type NeuralNet -contestant-file /ai/ai.json -output-path /ai/ai.json
