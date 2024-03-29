#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["shared/shared.csproj", "./shared/"]
COPY ["Directory.Packages.props", "."]
COPY ["src/HostedServiceXmlParser/HostedServiceXmlParser.csproj", "src/HostedServiceXmlParser/"]
COPY ["src/HostedServiceXmlParser/Files/status1.xml", "src/HostedServiceXmlParser/Files/"]
COPY ["src/EventBus.Messages/EventBus.Messages.csproj", "src/EventBus.Messages/"]
COPY ["src/RabbitMQ/RabbitMQ.csproj", "src/RabbitMQ/"]
RUN dotnet restore "./src/HostedServiceXmlParser/HostedServiceXmlParser.csproj"
COPY . .
WORKDIR "/src/src/HostedServiceXmlParser"
RUN dotnet build "./HostedServiceXmlParser.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HostedServiceXmlParser.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HostedServiceXmlParser.dll"]
