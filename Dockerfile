FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only project files first — leverages Docker layer caching 
COPY UniversityCourseManagement.slnx .
COPY src/University.API/*.csproj src/University.API/
COPY src/University.Application/*.csproj src/University.Application/
COPY src/University.Domain/*.csproj src/University.Domain/
COPY src/University.Persistance/*.csproj src/University.Persistance/
COPY src/University.Identity/*.csproj src/University.Identity/
# University.Tests is intentionally NOT copied here. 

RUN dotnet restore src/University.API/University.API.csproj

COPY . .
RUN dotnet publish src/University.API/University.API.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080   #container port
ENTRYPOINT ["dotnet", "University.API.dll"]
