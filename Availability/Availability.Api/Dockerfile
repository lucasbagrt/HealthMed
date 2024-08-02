FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 81
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Availability/Availability.Api/Availability.API.csproj", "Availability/Availability.Api/"]
COPY ["Common/HealthMed.CrossCutting/HealthMed.CrossCutting.csproj", "Common/HealthMed.CrossCutting/"]
COPY ["Common/HealthMed.Domain/HealthMed.Domain.csproj", "Common/HealthMed.Domain/"]
COPY ["Common/HealthMed.Service/HealthMed.Service.csproj", "Common/HealthMed.Service/"]
COPY ["Availability/Availability.Domain/Availability.Domain.csproj", "Availability/Availability.Domain/"]
COPY ["Availability/Availability.Infra.Data/Availability.Infra.Data.csproj", "Availability/Availability.Infra.Data/"]
COPY ["Common/HealthMed.Infra.Data/HealthMed.Infra.Data.csproj", "Common/HealthMed.Infra.Data/"]
COPY ["Availability/Availability.Service/Availability.Service.csproj", "Availability/Availability.Service/"]
RUN dotnet restore "./Availability/Availability.Api/./Availability.API.csproj"
COPY . .
WORKDIR "/src/Availability/Availability.Api"
RUN dotnet build "./Availability.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Availability.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:81
ENTRYPOINT ["dotnet", "Availability.API.dll"]