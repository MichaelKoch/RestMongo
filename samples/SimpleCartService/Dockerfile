#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["samples/SimpleCartService/SimpleCartService.csproj", "samples/SimpleCartService/"]
COPY ["RestMongo.Extensions/RestMongo.Extensions.csproj", "RestMongo.Extensions/"]
COPY ["RestMongo.Web/RestMongo.Web.csproj", "RestMongo.Web/"]
COPY ["RestMongo.Data/RestMongo.Data.csproj", "RestMongo.Data/"]
COPY ["RestMongo.Data.Abstractions/RestMongo.Data.Abstractions.csproj", "RestMongo.Data.Abstractions/"]
RUN dotnet restore "samples/SimpleCartService/SimpleCartService.csproj"
COPY . .
WORKDIR "/src/samples/SimpleCartService"
RUN dotnet build "SimpleCartService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleCartService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleCartService.dll"]