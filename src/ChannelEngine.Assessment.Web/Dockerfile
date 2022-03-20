#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ChannelEngine.Assessment.Web/ChannelEngine.Assessment.Web.csproj", "src/ChannelEngine.Assessment.Web/"]
RUN dotnet restore "src/ChannelEngine.Assessment.Web/ChannelEngine.Assessment.Web.csproj"
COPY . .
WORKDIR "/src/src/ChannelEngine.Assessment.Web"
RUN dotnet build "ChannelEngine.Assessment.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChannelEngine.Assessment.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChannelEngine.Assessment.Web.dll"]