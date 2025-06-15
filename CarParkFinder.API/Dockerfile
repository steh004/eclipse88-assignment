# Use the official .NET SDK image to build and publish
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY CarParkFinder.API/*.csproj CarParkFinder.API/
RUN dotnet restore CarParkFinder.API/CarParkFinder.API.csproj
COPY . .
WORKDIR /src/CarParkFinder.API
RUN dotnet build CarParkFinder.API.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish CarParkFinder.API.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY CarParkFinder.API/DataFiles ./DataFiles
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "CarParkFinder.API.dll"]
