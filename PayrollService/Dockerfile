FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "PayrollService.dll"]


# I am pulling  the SDK image and use it to build and publish source code into the /out directory.
#The /out directory contains published app files, including PayrollService.dll.

# Then I  pull the smaller ASP.NET runtime image and copy everything from /out of the SDK image into this runtime image.
# This runtime image is the one you actually run.

# Because the publish process trims unused dependencies and compiles your app into a self-contained set of files, the PayrollService.  dll and accompanying files in /out are much smaller and only include what the app truly needs to run.
# All the build tools, source code, and unnecessary dependencies are removed.

# Finally, the SDK build image is discarded after the build, so I keep only a small, clean, and optimized runtime image with the app ready to go.