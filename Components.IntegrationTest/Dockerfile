FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Components.IntegrationTest/Components.IntegrationTest.csproj", "Components.IntegrationTest/"]
RUN dotnet restore "Components.IntegrationTest/Components.IntegrationTest.csproj"
COPY . .
WORKDIR "/src/Components.IntegrationTest"
RUN dotnet build "Components.IntegrationTest.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Components.IntegrationTest.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Components.IntegrationTest.dll"]
