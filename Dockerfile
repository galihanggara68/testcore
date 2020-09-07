FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-bionic
WORKDIR /app
COPY ./ ./
ENTRYPOINT ["dotnet", "PDFExtractorAPI.dll"]