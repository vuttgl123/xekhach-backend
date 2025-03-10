# Sử dụng .NET SDK 8.0 để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy file .csproj và restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ source code và build ứng dụng
COPY . ./
RUN dotnet publish -c Release -o /publish

# Sử dụng .NET Runtime 8.0 để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy ứng dụng đã build từ bước trước
COPY --from=build /publish .

# Mở cổng 5120 để ứng dụng nhận request
EXPOSE 5120

# Cấu hình để ứng dụng lắng nghe đúng cổng Render cấp
ENV ASPNETCORE_URLS=http://+:5120

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "LuanAnTotNghiep_TuanVu_TuBac.dll"]
