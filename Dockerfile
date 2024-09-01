# syntax=docker/dockerfile:1

# Create a stage for building the application.
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

COPY . /source

WORKDIR /source/MvcMovie

# This is the architecture you’re building for, which is passed in by the builder.
# Placing it here allows the previous steps to be cached across architectures.
ARG TARGETARCH

# Build the application.
# Leverage a cache mount to /root/.nuget/packages so that subsequent builds don't have to re-download packages.
# If TARGETARCH is "amd64", replace it with "x64" - "x64" is .NET's canonical name for this and "amd64" doesn't
# work in .NET 6.0.
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

# Create a stage for managing node modules and compiling SCSS to CSS.
FROM node:14-alpine AS node_modules

# Set the working directory in the container
WORKDIR /source

# Copy only the package.json and package-lock.json files to the working directory
COPY MvcMovie/package*.json ./

# Install the node modules
RUN npm install

# Copy the SCSS files to the working directory
COPY MvcMovie/src/scss ./src/scss

# Compile SCSS to CSS
RUN npx sass src/scss/sass.scss wwwroot/css/sass.css

# Create the final stage for the runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Copy everything needed to run the app from the "build" stage.
COPY --from=build /app .

# Copy the compiled CSS from the node_modules stage.
COPY --from=node_modules /source/wwwroot/css /app/wwwroot/css

# Switch to a non-privileged user (defined in the base image) that the app will run under.
USER $APP_UID

ENTRYPOINT ["dotnet", "MvcMovie.dll"]
