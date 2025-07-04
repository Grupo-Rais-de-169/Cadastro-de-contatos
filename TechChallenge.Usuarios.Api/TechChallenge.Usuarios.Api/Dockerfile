FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallenge.Usuarios.Api/TechChallenge.Usuarios.Api/TechChallenge.Usuarios.Api.csproj", "TechChallenge.Usuarios.Api/TechChallenge.Usuarios.Api/"]
RUN dotnet restore "./TechChallenge.Usuarios.Api/TechChallenge.Usuarios.Api/TechChallenge.Usuarios.Api.csproj"
COPY . .
WORKDIR "/src/TechChallenge.Usuarios.Api/TechChallenge.Usuarios.Api"
RUN dotnet build "./TechChallenge.Usuarios.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge.Usuarios.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge.Usuarios.Api.dll"]