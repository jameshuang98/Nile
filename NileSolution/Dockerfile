# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the source code into the container
COPY . ./

# Build the application
RUN dotnet restore
RUN dotnet publish NileSolution.sln -c Release -o out

# Run stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port used by the application
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:5000
# Start the application
ENTRYPOINT ["dotnet", "Nile.Web.dll"]
