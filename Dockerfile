FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:35792ea4ad1db051981f62b313f1be3b46b1f45cadbaa3c288cd0d3056eefb83 AS build-env
WORKDIR /App

COPY ./WebShopSolution.sln ./

COPY ./WebShop.Repository/WebShop.Repository.csproj ./WebShop.Repository/
COPY ./WebShop.Services/WebShop.Services.csproj ./WebShop.Services/
COPY ./WebShop.API/WebShop.API.csproj ./WebShop.API/
COPY ./WebShop.Tests/WebShop.Tests.csproj ./WebShop.Tests/

RUN dotnet restore WebShopSolution.sln

COPY ./ ./

RUN dotnet test WebShop.Tests/WebShop.Tests.csproj --no-build --verbosity normal


RUN dotnet publish WebShop.API/WebShop.API.csproj -c Release -o out


# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0@sha256:6c4df091e4e531bb93bdbfe7e7f0998e7ced344f54426b7e874116a3dc3233ff
WORKDIR /App
COPY --from=build-env /App/out .

# Expose port and define entry point
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENTRYPOINT ["dotnet", "WebShop.API.dll"]