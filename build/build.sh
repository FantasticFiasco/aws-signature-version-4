#!/bin/bash

# Any subsequent(*) commands which fail will cause the shell script to exit immediately
set -e

# --- LOGO ---
echo -e "$(curl --silent https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.ansi)"
echo

# --- VARIABLES ---
GIT_SHA="${APPVEYOR_REPO_COMMIT:0:7}"
TAGGED_BUILD=$(( "$APPVEYOR_REPO_TAG" == "true" ? true : false ))
echo "[info] git sha: $GIT_SHA"
echo "[info] triggered by git tag: $TAGGED_BUILD"

# --- BUILD STAGE ---
echo "[build] build started"
echo "[build] dotnet cli v$(dotnet --version)"
VERSION_SUFFIX_ARG=$(( "$TAGGED_BUILD" == false ? "--version-suffix=$GIT_SHA" : "" ))
dotnet build -c Release "$VERSION_SUFFIX_ARG"
dotnet pack -c Release --include-symbols -o ../../artifacts --no-build "$VERSION_SUFFIX_ARG"

# --- TEST STAGE ---
echo "[test] test started"

# Exclude integration tests if we run as part of a pull requests. Integration tests rely on
# secrets, which are omitted by AppVeyor on pull requests.
TEST_FILTER=$(( "$APPVEYOR_PULL_REQUEST_NUMBER" != "" ? "--filter Category!=Integration" : "" ))
echo "[test] test filter: $TEST_FILTER"

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
