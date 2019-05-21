#!/bin/bash

# --- LOGO ---
echo -e "$(curl --silent https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.ansi)"
echo

# --- BUILD STAGE ---
echo "build: build started"
echo "build: dotnet cli v$(dotnet --version)"
dotnet build -c Release

# --- TEST STAGE ---
echo "test: test started"

# Exclude integration tests if we run as part of a pull requests, because integration tests rely on
# secrets omitted by AppVeyor on pull requests
if [ $APPVEYOR_PULL_REQUEST_NUMBER ]; then
    TEST_FILTER="--filter Category!=Integration"
fi

dotnet tool install --global coverlet.console
coverlet ./test/bin/Release/netcoreapp2.1/AWS.SignatureVersion4.Test.dll \
    --target "dotnet" \
    --targetargs "test --configuration Release --no-build $TEST_FILTER" \
    --exclude "[xunit.*]*" \
    --format opencover

echo "test: upload coverage report"

# Codecov expects environment variables CI and APPVEYOR to be "True", while it on AppVeyor Ubuntu
# images are specified as "true" (see https://www.appveyor.com/docs/environment-variables/ for
# reference). Lets remedy this by settig the environment variables ourself.
export CI="True"
export APPVEYOR="True"
bash <(curl -s https://codecov.io/bash)
