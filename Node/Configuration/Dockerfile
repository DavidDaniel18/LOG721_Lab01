#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Configuration/Configuration.csproj", "Configuration/"]
COPY ["Application.Commands.Mappers/Application.Commands.Mappers.csproj", "Application.Commands.Mappers/"]
COPY ["Application.Dtos/Application.Dtos.csproj", "Application.Dtos/"]
COPY ["Domain.Barycentre/Domain.Grouping.csproj", "Domain.Barycentre/"]
COPY ["Domain.Common/Domain.Common.csproj", "Domain.Common/"]
COPY ["Domain.Publicity/Domain.Publicity.csproj", "Domain.Publicity/"]
COPY ["Domain.Factories/Domain.Factories.csproj", "Domain.Factories/"]
COPY ["Application.Commands.Orchestrator/Application.Commands.Orchestrator.csproj", "Application.Commands.Orchestrator/"]
COPY ["Application.Commands/Application.Commands.csproj", "Application.Commands/"]
COPY ["Application.Common/Application.Common.csproj", "Application.Common/"]
COPY ["Application.Queries/Application.Queries.csproj", "Application.Queries/"]
COPY ["Infrastructure.Clients.Tcp/Infrastructure.Clients.Tcp.csproj", "Infrastructure.Clients.Tcp/"]
COPY ["Infrastructure.FileHandlers/Infrastructure.FileHandlers.csproj", "Infrastructure.FileHandlers/"]
COPY ["Presentation.Controllers.Rest/Presentation.Controllers.Rest.csproj", "Presentation.Controllers.Rest/"]
COPY ["Presentation.Controllers.Tcp/Presentation.Controllers.Tcp.csproj", "Presentation.Controllers.Tcp/"]
COPY ["Application.Commands.Map/Application.Commands.Map.csproj", "Application.Commands.Map/"]
COPY ["Domain.Services/Domain.Services.csproj", "Domain.Services/"]
COPY ["Application.Commands.Reducer/Application.Commands.Reducer.csproj", "Application.Commands.Reducer/"]
RUN dotnet restore "Configuration/Configuration.csproj"
COPY . .
WORKDIR "/src/Configuration"
RUN dotnet build "Configuration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Configuration.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Configuration.dll"]