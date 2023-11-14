# ใช้ .NET 7 SDK image เพื่อ build
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DIYshopAPI.csproj", "."]
RUN dotnet restore "DIYshopAPI.csproj"
COPY . .
WORKDIR "/src"

RUN dotnet build "DIYshopAPI.csproj" -c Release -o /app/build
RUN dotnet publish "DIYshopAPI.csproj" -c Release -o /app/publish


# เพิ่ม dotnet-ef โดยใช้คำสั่ง dotnet tool
RUN dotnet new tool-manifest
RUN dotnet tool install dotnet-ef --version 7.0.13 --no-cache 

# สร้าง migrations
RUN dotnet ef migrations script -o /app/migrations/UserdbContext.sql --context UserdbContext
RUN dotnet ef migrations script -o /app/migrations/CustomerContext.sql --context CustomerContext
RUN dotnet ef migrations script -o /app/migrations/ProductContext.sql --context ProductContext
RUN dotnet ef migrations script -o /app/migrations/OrderContext.sql --context OrderContext
RUN dotnet ef migrations script -o /app/migrations/OrderItemContext.sql --context OrderItemContext
RUN dotnet ef migrations script -o /app/migrations/PromotionContext.sql --context PromotionContext

# ใช้ .NET 7 runtime image เพื่อรันแอพ
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# คัดลอกไฟล์ที่ build มาจาก stage ก่อนหน้า
COPY --from=build /app/build .
COPY --from=build /app/migrations /app/migrations

# แก้ไข: ระบุการเชื่อมต่อกับ MSSQL ที่นี่
ENV ConnectionStrings__DefaultConnection "Server=localhost; Database=shopDIYdb; User Id=sa; Password=Twk13877; Trusted_Connection=False; Encrypt=True; TrustServerCertificate=True;"

# เพิ่ม SQL Server Edge
FROM mcr.microsoft.com/azure-sql-edge

# ให้คำสั่งเปลี่ยนสถานะเป็น shell เป็น PowerShell ที่ Docker image ของ SQL Server รองรับ
SHELL ["pwsh", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

# แก้ไข: กำหนดรหัสผ่าน root
ENV ACCEPT_EULA="Y" \
    SA_PASSWORD="Twk13877"

# สร้าง migrations
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $env:SA_PASSWORD -d shopDIYdb -i /app/migrations/UserdbContext.sql
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $env:SA_PASSWORD -d shopDIYdb -i /app/migrations/CustomerContext.sql
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $env:SA_PASSWORD -d shopDIYdb -i /app/migrations/ProductContext.sql
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $env:SA_PASSWORD -d shopDIYdb -i /app/migrations/OrderContext.sql
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $env:SA_PASSWORD -d shopDIYdb -i /app/migrations/OrderItemContext.sql
RUN /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $env:SA_PASSWORD -d shopDIYdb -i /app/migrations/PromotionContext.sql

# ในการเริ่มต้น SQL Server
CMD ["/opt/mssql/bin/sqlservr"]

# ใน stage final ของ .NET application
FROM base AS final
EXPOSE 8080
ENTRYPOINT ["dotnet", "DIYshopAPI.dll"]
