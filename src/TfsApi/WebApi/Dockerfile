#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/TfsApi/WebApi/WebApi.csproj", "src/TfsApi/WebApi/"]
COPY ["src/TfsApi/Application/Application.csproj", "src/TfsApi/Application/"]
COPY ["src/TfsApi/Domain/Domain.csproj", "src/TfsApi/Domain/"]
COPY ["src/TfsApi/Infrastructure/Infrastructure.csproj", "src/TfsApi/Infrastructure/"]
RUN dotnet restore "./src/TfsApi/WebApi/./WebApi.csproj"
COPY . .
WORKDIR "/src/src/TfsApi/WebApi"
RUN dotnet build "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApi.dll"]