#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Appointment/Appointment.API/Appointment.API.csproj", "Appointment/Appointment.API/"]
COPY ["Common/HealthMed.CrossCutting/HealthMed.CrossCutting.csproj", "Common/HealthMed.CrossCutting/"]
COPY ["Common/HealthMed.Domain/HealthMed.Domain.csproj", "Common/HealthMed.Domain/"]
COPY ["Common/HealthMed.Infra.Data/HealthMed.Infra.Data.csproj", "Common/HealthMed.Infra.Data/"]
COPY ["Common/HealthMed.Service/HealthMed.Service.csproj", "Common/HealthMed.Service/"]
COPY ["Appointment/Appointment.Domain/Appointment.Domain.csproj", "Appointment/Appointment.Domain/"]
COPY ["Appointment/Appointment.Infra.Data/Appointment.Infra.Data.csproj", "Appointment/Appointment.Infra.Data/"]
COPY ["Appointment/Appointment.Service/Appointment.Service.csproj", "Appointment/Appointment.Service/"]
RUN dotnet restore "./Appointment/Appointment.API/Appointment.API.csproj"
COPY . .
WORKDIR "/src/Appointment/Appointment.API"
ARG BUILD_CONFIGURATION=Release
RUN dotnet build "./Appointment.API.csproj" -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Appointment.API.csproj" -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointment.API.dll"]