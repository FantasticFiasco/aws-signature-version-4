#!/bin/bash

echo -e "$(curl --silent https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.ansi)"
echo

echo "build: build started"
echo "build: dotnet cli v$(dotnet --version)"
dotnet build -c Release

echo "test: test started"
dotnet tool install --global coverlet.console
coverlet ./test/bin/Release/netcoreapp2.1/AWS.SignatureVersion4.Test.dll \
    --target "dotnet" \
    --targetargs "test --configuration Release --no-build" \
    --exclude "[xunit.*]*" \
    --format opencover

echo "test: upload coverage report"
# Codecov expects environment variables CI and APPVEYOR to be "True", while it on AppVeyor Ubuntu
# images are specified as "true" (see https://www.appveyor.com/docs/environment-variables/ for
# reference). Lets remedy this by settig the environment variables ourself.
export CI="True"
export APPVEYOR="True"
bash <(curl -s https://codecov.io/bash)
