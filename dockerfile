FROM microsoft/dotnet:2.1-sdk

ARG branch=master
ARG generationCount=20
ARG iterationCount=100

RUN apt-get update
RUN apt-get install -y unzip libunwind8 gettext
RUN mkdir Draughts
ADD https://github.com/RichTeaMan/Draughts/archive/$branch.tar.gz draughts.tar.gz
RUN tar -xzf draughts.tar.gz --strip-components=1 -C Draughts
WORKDIR /Draughts
RUN ./cake.sh -target=build
ENTRYPOINT ./cake.sh -target=Train
CMD []
