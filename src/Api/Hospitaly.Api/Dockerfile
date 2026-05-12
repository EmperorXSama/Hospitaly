# https://aka.ms/dotenc/imagenet/dockermanifest
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
RUN apt-get update && apt-get install -y --no-install-recommends \
    curl \
    && rm -rf /var/lib/apt/lists/*
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG LAUNCH_PROFILE=Development
WORKDIR /src
COPY src/Common/Hospitaly.Common.Domain/Hospitaly.Common.Domain.csproj src/Common/Hospitaly.Common.Domain/
COPY src/Common/Hospitaly.Common.Application/Hospitaly.Common.Application.csproj src/Common/Hospitaly.Common.Application/
COPY src/Common/Hospitaly.Common.Infrastructure/Hospitaly.Common.Infrastructure.csproj src/Common/Hospitaly.Common.Infrastructure/
COPY src/Common/Hospitaly.Common.Presentation/Hospitaly.Common.Presentation.csproj src/Common/Hospitaly.Common.Presentation/
COPY src/Modules/Users/Hospitaly.Modules.Users.Domain/Hospitaly.Modules.Users.Domain.csproj src/Modules/Users/Hospitaly.Modules.Users.Domain/
COPY src/Modules/Users/Hospitaly.Modules.Users.Application/Hospitaly.Modules.Users.Application.csproj src/Modules/Users/Hospitaly.Modules.Users.Application/
COPY src/Modules/Users/Hospitaly.Modules.Users.Infrastructure/Hospitaly.Modules.Users.Infrastructure.csproj src/Modules/Users/Hospitaly.Modules.Users.Infrastructure/
COPY src/Modules/Users/Hospitaly.Modules.Users.Presentation/Hospitaly.Modules.Users.Presentation.csproj src/Modules/Users/Hospitaly.Modules.Users.Presentation/
COPY src/Modules/Cliniks/Hospitaly.Modules.Clinic.Domain/Hospitaly.Modules.Clinic.Domain.csproj src/Modules/Cliniks/Hospitaly.Modules.Clinic.Domain/
COPY src/Modules/Cliniks/Hospitaly.Modules.Clinic.Application/Hospitaly.Modules.Clinic.Application.csproj src/Modules/Cliniks/Hospitaly.Modules.Clinic.Application/
COPY src/Modules/Cliniks/Hospitaly.Modules.Clinic.Infrastructure/Hospitaly.Modules.Clinic.Infrastructure.csproj src/Modules/Cliniks/Hospitaly.Modules.Clinic.Infrastructure/
COPY src/Modules/Cliniks/Hospitaly.Modules.Clinic.Presentation/Hospitaly.Modules.Clinic.Presentation.csproj src/Modules/Cliniks/Hospitaly.Modules.Clinic.Presentation/
COPY src/Api/Hospitaly.Api/Hospitaly.Api.csproj src/Api/Hospitaly.Api/
RUN dotnet restore src/Api/Hospitaly.Api/Hospitaly.Api.csproj
COPY . .
WORKDIR src/Api/Hospitaly.Api
RUN dotnet build "./Hospitaly.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Hospitaly.Api.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false \
    /p:PublishTrimmed=false \
    /p:StripSymbols=true

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
HEALTHCHECK --interval=30s --timeout=3s --start-period=60s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1
ENTRYPOINT ["dotnet", "Hospitaly.Api.dll"]
