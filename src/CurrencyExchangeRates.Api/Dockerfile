#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/CurrencyExchangeRates.Api/CurrencyExchangeRates.Api.csproj", "CurrencyExchangeRates.Api/"]
RUN dotnet restore "CurrencyExchangeRates.Api/CurrencyExchangeRates.Api.csproj"
COPY . .
WORKDIR "/src/CurrencyExchangeRates.Api"
RUN dotnet build "CurrencyExchangeRates.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurrencyExchangeRates.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurrencyExchangeRates.Api.dll"]