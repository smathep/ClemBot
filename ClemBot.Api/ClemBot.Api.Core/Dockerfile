FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ClemBot.Api.Core/ClemBot.Api.Core.csproj", "ClemBot.Api.Core/"]
RUN dotnet restore "ClemBot.Api.Core/ClemBot.Api.Core.csproj"
COPY . .
WORKDIR "/src/ClemBot.Api.Core"
RUN dotnet build "ClemBot.Api.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ClemBot.Api.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ClemBot.Api.Core.dll"]
