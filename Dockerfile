# Final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as base
WORKDIR /app

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
ARG RELEASE_VERSION
WORKDIR /src

COPY ["src/Trove/Trove.csproj", "Trove/"]
COPY ["src/Trove.DataAccess.FileSystem/Trove.DataAccess.FileSystem.csproj", "Trove.DataAccess.FileSystem/"]
COPY ["src/Trove.DataAccess.MongoDB/Trove.DataAccess.MongoDB.csproj", "Trove.DataAccess.MongoDB/"]
COPY ["src/Trove.DataModels/Trove.DataModels.csproj", "Trove.DataModels/"]
COPY ["src/Trove.Shared/Trove.Shared.csproj", "Trove.Shared/"]

RUN dotnet restore "Trove/Trove.csproj"
COPY . .
WORKDIR "/src/Trove"
RUN dotnet build "Trove.csproj" -c Release -o /app/build -p:VersionPrefix=$RELEASE_VERSION

FROM build AS publish
RUN dotnet publish "Trove.csproj" -c Release -o /app/publish -p:VersionPrefix=$RELEASE_VERSION

# Copy artifacts to final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trove.dll"]