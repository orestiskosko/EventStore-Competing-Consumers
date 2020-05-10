FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

COPY ./CompetingConsumers.Common ./CompetingConsumers.Common
COPY ./CompetingConsumers.EventProducer ./CompetingConsumers.EventProducer

RUN dotnet restore ./CompetingConsumers.EventProducer/CompetingConsumers.EventProducer.csproj

RUN dotnet build ./CompetingConsumers.EventProducer/CompetingConsumers.EventProducer.csproj

RUN dotnet publish ./CompetingConsumers.EventProducer/CompetingConsumers.EventProducer.csproj -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "CompetingConsumers.EventProducer.dll"]
