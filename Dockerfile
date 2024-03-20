FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ob-backend/*", "ob-backend/"]

ENV PATH="${PATH}:/root/.dotnet/tools"