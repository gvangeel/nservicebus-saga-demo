#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/NServiceBus.Saga.Demo.TransportationService/NServiceBus.Saga.Demo.TransportationService.csproj", "NServiceBus.Saga.Demo.TransportationService/"]
COPY ["src/NServiceBus.Saga.Demo.Contracts/NServiceBus.Saga.Demo.Contracts.csproj", "NServiceBus.Saga.Demo.Contracts/"]
RUN dotnet restore "NServiceBus.Saga.Demo.TransportationService/NServiceBus.Saga.Demo.TransportationService.csproj"
COPY ./src/NServiceBus.Saga.Demo.TransportationService/ ./NServiceBus.Saga.Demo.TransportationService/
COPY ./src/NServiceBus.Saga.Demo.Contracts/ ./NServiceBus.Saga.Demo.Contracts/
WORKDIR "/src/NServiceBus.Saga.Demo.TransportationService"
RUN dotnet build "NServiceBus.Saga.Demo.TransportationService.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "NServiceBus.Saga.Demo.TransportationService.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NServiceBus.Saga.Demo.TransportationService.dll"]