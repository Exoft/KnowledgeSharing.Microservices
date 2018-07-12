FROM microsoft/dotnet:2.1.300-sdk AS build-env
WORKDIR /app

COPY . ./
RUN dotnet restore

RUN dotnet publish -c Release -o out

EXPOSE 5001

WORKDIR BookingService.Api/out
ENTRYPOINT ["dotnet", "BookingService.Api.dll"]