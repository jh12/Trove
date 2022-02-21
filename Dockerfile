# Final image
FROM mcr.microsoft.com/dotnet/aspnet:6.0-bullseye-slim as base
WORKDIR /app

# Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0-bullseye-slim AS build
ARG RELEASE_VERSION
WORKDIR /sln

COPY ./*.sln ./

# Copy the main source project files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

# Copy the test project files
#COPY test/*/*.csproj ./
#RUN for file in $(ls *.csproj); do mkdir -p test/${file%.*}/ && mv $file test/${file%.*}/; done

RUN dotnet restore

#COPY ./test ./test
COPY ./src ./src
RUN dotnet build -c Release --no-restore -o /app/build -p:VersionPrefix=$RELEASE_VERSION

RUN dotnet publish "./src/Trove/Trove.csproj" -c Release --no-restore -o /app/publish -p:VersionPrefix=$RELEASE_VERSION

# Copy artifacts to final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Trove.dll"]