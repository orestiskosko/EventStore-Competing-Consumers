﻿FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY ./CompetingConsumers.Common ./CompetingConsumers.Common
COPY ./CompetingConsumers.EventConsumer ./CompetingConsumers.EventConsumer

RUN dotnet restore ./CompetingConsumers.EventConsumer/CompetingConsumers.EventConsumer.csproj

RUN dotnet build ./CompetingConsumers.EventConsumer/CompetingConsumers.EventConsumer.csproj

RUN dotnet publish ./CompetingConsumers.EventConsumer/CompetingConsumers.EventConsumer.csproj -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "CompetingConsumers.EventConsumer.dll"]
