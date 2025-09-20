# ===============================
# Stage 1: Build
# ===============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# انسخ كل الملفات (السوليوشن + المشاريع + props/targets)
COPY . .

# Restore من السوليوشن
RUN dotnet restore "Dev.Acadmy.sln"

# Build + Publish لمشروع الـ Host
WORKDIR /src/src/Dev.Acadmy.HttpApi.Host
RUN dotnet publish "Dev.Acadmy.HttpApi.Host.csproj" -c Release -o /app/publish

# ===============================
# Stage 2: Runtime
# ===============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# انسخ ملفات النشر
COPY --from=build /app/publish .

# انسخ ملف الشهادة داخل الحاوية (تأكد من مكانه في مشروعك)
COPY src/Dev.Acadmy.HttpApi.Host/openiddict.pfx /app/openiddict.pfx

# اضبط متغير البيئة لمسار الشهادة وكلمة المرور
ENV OPENIDDICT_CERT_PATH=/app/openiddict.pfx
ENV OPENIDDICT_CERT_PASSWORD=010203  # غيرها لكلمة المرور الفعلية

ENTRYPOINT ["dotnet", "Dev.Acadmy.HttpApi.Host.dll"]
