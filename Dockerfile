# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore "TareasApi/TareasApi.csproj"

RUN dotnet publish "TareasApi/TareasApi.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# Etapa de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000

EXPOSE 10000

ENTRYPOINT ["dotnet", "TareasApi.dll"]