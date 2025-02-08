# FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
# WORKDIR /app
# EXPOSE 80 443

# ENV ASPNETCORE_URLS=http://+:443

# FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
# ARG configuration=Release
# WORKDIR /src
# COPY ["TodoApi/TodoApi.csproj", "TodoApi/"]
# RUN dotnet restore "TodoApi/TodoApi.csproj"
# COPY . .
# WORKDIR "/src/TodoApi"
# RUN dotnet build "TodoApi.csproj" -c $configuration -o /app/build

# FROM build AS publish
# ARG configuration=Release
# RUN dotnet publish "TodoApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "TodoApi.dll"]
# Base image for the app with ASP.NET runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0-preview AS base
WORKDIR /app
EXPOSE 80 443

# Set ASP.NET Core to listen on both HTTP and HTTPS
ENV ASPNETCORE_URLS=http://+:80;https://+:443

# Build the application using the SDK image
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0-preview AS build
ARG configuration=Release
WORKDIR /src
COPY ["TodoApi/TodoApi.csproj", "TodoApi/"]
RUN dotnet restore "TodoApi/TodoApi.csproj"
COPY . . 
WORKDIR "/src/TodoApi"
RUN dotnet build "TodoApi.csproj" -c $configuration -o /app/build

# Publish the application
FROM build AS publish
ARG configuration=Release
RUN dotnet publish "TodoApi.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

# Final image with the app ready to run
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish . 
ENTRYPOINT ["dotnet", "TodoApi.dll"]
