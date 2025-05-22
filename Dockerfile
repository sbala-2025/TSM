FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TSM.API/TSM.API.csproj", "TSM.API/"]
COPY ["TSM.Core/TSM.Core.csproj", "TSM.Core/"]
COPY ["TSM.Infrastructure/TSM.Infrastructure.csproj", "TSM.Infrastructure/"]
RUN dotnet restore "TSM.API/TSM.API.csproj"
COPY . .
WORKDIR "/src/TSM.API"
RUN dotnet build "TSM.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TSM.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TSM.API.dll"] 