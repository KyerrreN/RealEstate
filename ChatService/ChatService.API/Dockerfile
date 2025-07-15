# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0-noble AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0-noble AS build
ARG BUILD_CONFIGURATION=Release
ARG GITHUB_USERNAME
ARG GITHUB_TOKEN
ENV GITHUB_USERNAME=${GITHUB_USERNAME}
ENV GITHUB_TOKEN=${GITHUB_TOKEN}
WORKDIR /src
COPY ["ChatService.API/ChatService.API.csproj", "ChatService.API/"]
COPY ["ChatService.BLL/ChatService.BLL.csproj", "ChatService.BLL/"]
COPY ["ChatService.DAL/ChatService.DAL.csproj", "ChatService.DAL/"]
COPY ["ChatService.Domain/ChatService.Domain.csproj", "ChatService.Domain/"]
RUN dotnet nuget add source --username $GITHUB_USERNAME --password $GITHUB_TOKEN --store-password-in-clear-text --name github "https://nuget.pkg.github.com/${GITHUB_USERNAME}/index.json"
RUN dotnet restore "./ChatService.API/ChatService.API.csproj"
COPY . .
WORKDIR "/src/ChatService.API"
RUN dotnet build "./ChatService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChatService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChatService.API.dll"]