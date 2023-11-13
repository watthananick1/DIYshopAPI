# ใช้ .NET 7 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 5210

# ใช้ .NET 7 SDK image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DIYshopAPI.csproj", "."]
RUN dotnet restore "DIYshopAPI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "DIYshopAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DIYshopAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# แก้ไข: ระบุการเชื่อมต่อกับ MSSQL ที่นี่
ENV ConnectionStrings__DefaultConnection "Server=localhost; Database=shopDIYdb; User Id=sa; Password=Twk13877; Trusted_Connection=False; Encrypt=True; TrustServerCertificate=True;"

ENTRYPOINT ["dotnet", "DIYshopAPI.dll"]
