#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Vertical.Product.Service.Api/Vertical.Product.Service.Api.csproj", "Vertical.Product.Service.Api/"]
RUN dotnet restore "Vertical.Product.Service.Api/Vertical.Product.Service.Api.csproj"
COPY . .
WORKDIR "/src/Vertical.Product.Service.Api"
RUN dotnet build "Vertical.Product.Service.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vertical.Product.Service.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vertical.Product.Service.Api.dll"]