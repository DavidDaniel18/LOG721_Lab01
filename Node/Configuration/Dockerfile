#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Configuration/ConfigurationNode.csproj", "Configuration/"]
COPY ["Application.Commands.Mappers/Application.Commands.Mappers.csproj", "Application.Commands.Mappers/"]
COPY ["Application.Dtos/Application.Dtos.csproj", "Application.Dtos/"]
COPY ["Domain.Barycentre/Domain.Grouping.csproj", "Domain.Barycentre/"]
COPY ["Domain.Common/DomainNode.Common.csproj", "Domain.Common/"]
COPY ["Domain.Publicity/Domain.Publicity.csproj", "Domain.Publicity/"]
COPY ["Domain.Factories/Domain.Factories.csproj", "Domain.Factories/"]
COPY ["Application.Commands.Map/Application.Commands.Map.csproj", "Application.Commands.Map/"]
COPY ["Application.Commands.Orchestrator/Application.Commands.Orchestrator.csproj", "Application.Commands.Orchestrator/"]
COPY ["Application.Commands/Application.Commands.csproj", "Application.Commands/"]
COPY ["Application.Common/Application.Common.csproj", "Application.Common/"]
COPY ["Application.Queries/Application.Queries.csproj", "Application.Queries/"]
COPY ["Domain.Services/DomainNode.Services.csproj", "Domain.Services/"]
COPY ["Application.Commands.Reducer/Application.Commands.Reducer.csproj", "Application.Commands.Reducer/"]
COPY ["Domain.Orchestrating/Domain.Orchestrating.csproj", "Domain.Orchestrating/"]
COPY ["Infrastructure.Cache/Infrastructure.Cache.csproj", "Infrastructure.Cache/"]
COPY ["Infrastructure.Clients.Rest/Infrastructure.Clients.Rest.csproj", "Infrastructure.Clients.Rest/"]
COPY ["Infrastructure.Clients.Tcp/Infrastructure.Clients.Tcp.csproj", "Infrastructure.Clients.Tcp/"]
COPY ["Infrastructure.FileHandlers/Infrastructure.FileHandlers.csproj", "Infrastructure.FileHandlers/"]
COPY ["Infrastructure.Write.Repositories/Infrastructure.Write.Repositories.csproj", "Infrastructure.Write.Repositories/"]
COPY ["Presentation.Controllers.Rest/Presentation.Controllers.Rest.csproj", "Presentation.Controllers.Rest/"]
COPY ["Presentation.Controllers.Tcp/Presentation.Controllers.Tcp.csproj", "Presentation.Controllers.Tcp/"]
RUN dotnet restore "Configuration/ConfigurationNode.csproj"
COPY . .
WORKDIR "/src/Configuration"
RUN dotnet build "ConfigurationNode.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConfigurationNode.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConfigurationNode.dll"]