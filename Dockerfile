# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar el proyecto específico
COPY TaskFlow/TaskFlow.csproj ./TaskFlow/
WORKDIR /app/TaskFlow
RUN dotnet restore

# Copiar todo el código
WORKDIR /app
COPY . .

WORKDIR /app/TaskFlow
RUN dotnet publish -c Release -o /app/out

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "TaskFlow.dll"]
