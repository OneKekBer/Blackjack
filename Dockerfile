FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blackjack.Presentation/Blackjack.Presentation.csproj", "Blackjack.Presentation/"]
COPY ["Blackjack.Data/Blackjack.Data.csproj", "Blackjack.Data/"]
COPY ["Blackjack.Business/Blackjack.Business.csproj", "Blackjack.Business/"]
RUN dotnet restore "./Blackjack.Presentation/Blackjack.Presentation.csproj"
COPY . .
WORKDIR "/src/Blackjack.Presentation"
RUN dotnet build "./Blackjack.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Blackjack.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Blackjack.Presentation.dll"]