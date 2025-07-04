FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TechChallenge.Cadastro.Api/TechChallenge.Cadastro.Api.csproj", "TechChallenge.Cadastro.Api/"]
COPY ["TechChallenge.Core/TechChallenge.Core.csproj", "TechChallenge.Core/"]
RUN dotnet restore "./TechChallenge.Cadastro.Api/TechChallenge.Cadastro.Api.csproj"
COPY . .
WORKDIR "/src/TechChallenge.Cadastro.Api"
RUN dotnet build "./TechChallenge.Cadastro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TechChallenge.Cadastro.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TechChallenge.Cadastro.Api.dll"]