# ===== BUILD =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Daily food/Daily food.csproj", "Daily food/"]
COPY ["Aplicacion/Aplicacion.csproj", "Aplicacion/"]
COPY ["Dominio/Dominio.csproj", "Dominio/"]
COPY ["Infraestructura/Infraestructura.csproj", "Infraestructura/"]
RUN dotnet restore "Daily food/Daily food.csproj"

COPY . .
RUN dotnet publish "Daily food/Daily food.csproj" -c Release -o /app/publish

# ===== RUNTIME =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["/bin/sh", "-c", "dotnet 'Daily food.dll' --urls http://0.0.0.0:$PORT"]