FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["HyperledgerTest/VeroAPI/VeroAPI.csproj", "HyperledgerTest/VeroAPI/"]
RUN dotnet restore "HyperledgerTest/VeroAPI/VeroAPI.csproj"
COPY . .
WORKDIR "/src/HyperledgerTest/VeroAPI"
RUN dotnet build "VeroAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VeroAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VeroAPI.dll"]