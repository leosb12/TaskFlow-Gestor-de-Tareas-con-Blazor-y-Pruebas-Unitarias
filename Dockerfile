# Etapa de compilaci�n
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el contenido
COPY . .

# Restaurar dependencias
RUN dotnet restore

# Publicar en modo Release a la carpeta /app
RUN dotnet publish -c Release -o /app

# Etapa de ejecuci�n
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar desde la build
COPY --from=build /app .

# Puerto expuesto (Railway lo detecta autom�ticamente si us�s el puerto 80)
EXPOSE 80

# Iniciar la app (cambi� TaskFlow.dll si tu proyecto se llama distinto)
ENTRYPOINT ["dotnet", "TaskFlow.dll"]
