FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS release

COPY . /app

WORKDIR /app

RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:5.0 AS runtime

WORKDIR /app

COPY --from=release /app/out .

ENTRYPOINT ["dotnet", "BragaPets.API.dll"]

EXPOSE 80
