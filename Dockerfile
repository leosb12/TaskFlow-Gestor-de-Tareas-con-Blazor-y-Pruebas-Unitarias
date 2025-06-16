FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY *.sln ./
COPY TaskFlow/*.csproj TaskFlow/
COPY TaskFlow.Tests/*.csproj TaskFlow.Tests/


RUN dotnet restore TaskFlow.sln


COPY . .

RUN dotnet publish TaskFlow.sln -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TaskFlow.dll"]
