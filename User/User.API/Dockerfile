FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

WORKDIR /app
EXPOSE 82
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["User/User.API/User.API.csproj", "User/User.API/"]
COPY ["Common/HealthMed.CrossCutting/HealthMed.CrossCutting.csproj", "Common/HealthMed.CrossCutting/"]
COPY ["Common/HealthMed.Domain/HealthMed.Domain.csproj", "Common/HealthMed.Domain/"]
COPY ["Common/HealthMed.Service/HealthMed.Service.csproj", "Common/HealthMed.Service/"]
COPY ["User/User.Domain/User.Domain.csproj", "User/User.Domain/"]
COPY ["User/User.Infra.Data/User.Infra.Data.csproj", "User/User.Infra.Data/"]
COPY ["Common/HealthMed.Infra.Data/HealthMed.Infra.Data.csproj", "Common/HealthMed.Infra.Data/"]
COPY ["User/User.Service/User.Service.csproj", "User/User.Service/"]
RUN dotnet restore "./User/User.API/./User.API.csproj"
COPY . .
WORKDIR "/src/User/User.API"
RUN dotnet build "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./User.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:82
ENTRYPOINT ["dotnet", "User.API.dll"]