#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["HiperMicroservice/HiperMicroservice.csproj", "HiperMicroservice/"]
RUN dotnet restore "HiperMicroservice/HiperMicroservice.csproj"
COPY . .
WORKDIR "/src/HiperMicroservice"
RUN dotnet build "HiperMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HiperMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HiperMicroservice.dll"]