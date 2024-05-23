FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER root
RUN apt-get update && apt-get install -y sudo && apt-get install -y libgssapi-krb5-2
RUN apt-get clean
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CryptoChecker.API/CryptoChecker.API.csproj", "CryptoChecker.API/"]
COPY ["CryptoChecker.API/CryptoChecker.Domain.csproj", "CryptoChecker.Domain/"]
COPY ["CryptoChecker.API/CryptoChecker.Infrastructure.csproj", "CryptoChecker.Infrastructure/"]
COPY ["CryptoChecker.API/CryptoChecker.Application.csproj", "CryptoChecker.Application/"]
RUN dotnet restore "./CryptoChecker.API/./CryptoChecker.API.csproj"
COPY . .
WORKDIR "/src/CryptoChecker.API"
RUN dotnet build "./CryptoChecker.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CryptoChecker.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoChecker.API.dll"]