FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DocumentIntelligence.API/DocumentIntelligence.API.csproj", "DocumentIntelligence.API/"]
RUN dotnet restore "DocumentIntelligence.API/DocumentIntelligence.API.csproj"
COPY . .
WORKDIR "/src/DocumentIntelligence.API"
RUN dotnet build "./DocumentIntelligence.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DocumentIntelligence.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DocumentIntelligence.API.dll"]
