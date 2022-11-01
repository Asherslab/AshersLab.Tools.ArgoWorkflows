FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AshersLab.Tools.ArgoWorkflows/AshersLab.Tools.ArgoWorkflows.csproj", "AshersLab.Tools.ArgoWorkflows/"]
RUN dotnet restore "AshersLab.Tools.ArgoWorkflows/AshersLab.Tools.ArgoWorkflows.csproj"
COPY . .
WORKDIR "/src/AshersLab.Tools.ArgoWorkflows"
RUN dotnet build "AshersLab.Tools.ArgoWorkflows.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AshersLab.Tools.ArgoWorkflows.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AshersLab.Tools.ArgoWorkflows.dll"]
