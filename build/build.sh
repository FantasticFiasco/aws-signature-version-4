#!/bin/bash

# Any subsequent(*) commands which fail will cause the shell script to exit immediately
set -e

# --- LOGO ---
echo -e "$(curl --silent https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.ansi)"
echo

# --- VARIABLES ---
TAGGED_BUILD=$(( "$APPVEYOR_REPO_TAG" == "true" ? true : false ))
GIT_SHA="${APPVEYOR_REPO_COMMIT:0:7}"
echo "[info] triggered by git tag: $TAGGED_BUILD"
echo "[info] git sha: $GIT_SHA"

# --- BUILD STAGE ---
echo "[build] build started"
echo "[build] dotnet cli v$(dotnet --version)"
dotnet build -c Release

# --- TEST STAGE ---
echo "[test] test started"

# Exclude integration tests if we run as part of a pull requests. Integration tests rely on
# secrets, which are omitted by AppVeyor on pull requests.
if [ $APPVEYOR_PULL_REQUEST_NUMBER ]; then
    echo "[test] skip integration tests on pull requests"
    TEST_FILTER="--filter Category!=Integration"
fi

dotnet tool install --global coverlet.console
coverlet ./test/bin/Release/netcoreapp2.1/AWS.SignatureVersion4.Test.dll \
    --target "dotnet" \
    --targetargs "test --configuration Release --no-build $TEST_FILTER" \
    --exclude "[xunit.*]*" \
    --format opencover

echo "[test] upload coverage report"

# Codecov expects environment variables CI and APPVEYOR to be "True", while it on AppVeyor Ubuntu
# images are specified as "true" (see https://www.appveyor.com/docs/environment-variables/ for
# reference). Lets remedy this by settig the environment variables ourself.
export CI="True"
export APPVEYOR="True"
bash <(curl -s https://codecov.io/bash)
