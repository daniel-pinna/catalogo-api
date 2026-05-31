# Estágio 1: Base de execução (runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Estágio 2: SDK para compilação e restauração
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["CatalogoApi.csproj", "."]
RUN dotnet restore "CatalogoApi.csproj"
COPY . .
RUN dotnet publish "CatalogoApi.csproj" -c Release -o /app/build

# Estágio 3: Publicação dos arquivos (AQUI ESTAVA O ERRO)
FROM build AS publish
RUN dotnet publish "CatalogoApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio 4: Imagem final limpa e otimizada
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatalogoApi.dll"]