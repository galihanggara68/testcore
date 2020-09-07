FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore "PDFExtractorAPI.csproj"
RUN dotnet publish "PDFExtractorAPI.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV TZ=Asia/Jakarta
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PDFExtractorAPI.dll"]