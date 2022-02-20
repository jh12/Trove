# Final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as base
WORKDIR /app

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["src/Trove/Trove.csproj", "Trove/"]
COPY ["src/Trove.DataAccess.FileSystem/Trove.DataAccess.FileSystem.csproj", "TroveDataAccessFileSystem/"]
COPY ["src/Trove.DataAccess.MongoDB/Trove.DataAccess.MongoDB.csproj", "TroveDataAccessMongoDB/"]
COPY ["src/Trove.DataModels/Trove.DataModels.csproj", "TroveDataModels/"]
COPY ["src/Trove.Shared/Trove.Shared.csproj", "TroveShared/"]

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