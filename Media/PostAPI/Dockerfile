#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Media/PostAPI/PostAPI.csproj", "Media/PostAPI/"]
RUN dotnet restore "Media/PostAPI/PostAPI.csproj"
COPY . .

# RUN dotnet tool install --global dotnet-ef --version 7.*
# ENV PATH="${PATH}:/root/.dotnet/tools"

WORKDIR "/src/Media/PostAPI"
RUN dotnet build "PostAPI.csproj" -c Release -o /app/build

# RUN dotnet ef migrations add InitialCreatePost --context PostDbContext
# RUN dotnet ef database update -c PostDbContext

FROM build AS publish
RUN dotnet publish "PostAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "PostAPI.dll"]