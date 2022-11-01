FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base

ARG publish
ARG project

WORKDIR /app
COPY ${publish} .
ENTRYPOINT ["dotnet", "AshersLab.Tools.ArgoWorkflows.dll"]
