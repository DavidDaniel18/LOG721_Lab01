#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Configuration/Configuration.csproj", "Configuration/"]
COPY ["Controlleur/Controllers.csproj", "Controlleur/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Entities/Entities.csproj", "Entities/"]
COPY ["Interfaces/Interfaces.csproj", "Interfaces/"]
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