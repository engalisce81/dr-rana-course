# استخدم ASP.NET runtime كأساس
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# استخدم SDK للبناء
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Dev.Acadmy.HttpApi.Host/Dev.Acadmy.HttpApi.Host.csproj", "Dev.Acadmy.HttpApi.Host/"]
RUN dotnet restore "Dev.Acadmy.HttpApi.Host/Dev.Acadmy.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/Dev.Acadmy.HttpApi.Host"
RUN dotnet publish "Dev.Acadmy.HttpApi.Host.csproj" -c Release -o /app/publish

# بناء الصورة النهائية
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Dev.Acadmy.HttpApi.Host.dll"]
