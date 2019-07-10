#!/bin/bash

# Any subsequent(*) commands which fail will cause the shell script to exit immediately
set -e

# -------------------------------------------------------------------------------------------------
# LOGO
# -------------------------------------------------------------------------------------------------
echo -e "`curl --silent https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.ansi`\n"

# -------------------------------------------------------------------------------------------------
# VARIABLES
# -------------------------------------------------------------------------------------------------
GIT_SHA="${APPVEYOR_REPO_COMMIT:0:7}"
[ "${APPVEYOR_REPO_TAG}" = "true" ] && IS_TAGGED_BUILD=true || IS_TAGGED_BUILD=false
[ ! -z "${APPVEYOR_PULL_REQUEST_NUMBER}" ] && IS_PULL_REQUEST=true || IS_PULL_REQUEST=false
echo "[info] git sha: ${GIT_SHA}"
echo "[info] is git tag: ${IS_TAGGED_BUILD}"
echo "[info] is pull request: ${IS_PULL_REQUEST}"

# -------------------------------------------------------------------------------------------------
# BUILD
# -------------------------------------------------------------------------------------------------
echo "[build] build started"
echo "[build] dotnet cli v`dotnet --version`"

# echo "xbuild cli"
# xbuild AwsSignatureVersion4.sln

echo "msbuild cli"
msbuild --version

echo "dotnet cli"
[ "${IS_TAGGED_BUILD}" = false ] && VERSION_SUFFIX_ARG="--version-suffix=sha-${GIT_SHA}"
dotnet build -c Release "${VERSION_SUFFIX_ARG}"
dotnet pack -c Release --include-symbols -o ./../artifacts --no-build "${VERSION_SUFFIX_ARG}"

# -------------------------------------------------------------------------------------------------
# TEST
# -------------------------------------------------------------------------------------------------
echo "[test] test started"

# Exclude integration tests if we run as part of a pull requests. Integration tests rely on
# secrets, which are omitted by AppVeyor on pull requests.
[ "${IS_PULL_REQUEST}" = true ] && TEST_FILTER="--filter Category!=Integration"
echo "[test] test filter: ${TEST_FILTER}"

dotnet tool install --global coverlet.console
coverlet ./test/bin/Release/netcoreapp2.1/AwsSignatureVersion4.Test.dll \
    --target "dotnet" \
    --targetargs "test --configuration Release --no-build ${TEST_FILTER}" \
    --exclude "[xunit.*]*" \
    --format opencover

# Codecov expects environment variables "CI" and "APPVEYOR" to be "True", while it on AppVeyor
# Ubuntu images are specified as "true" (see https://www.appveyor.com/docs/environment-variables/
# for reference). Lets remedy this by settig the environment variables ourself.
if [ "${IS_PULL_REQUEST}" = false ]; then
    echo "[test] upload coverage report"
    export CI="True"
    export APPVEYOR="True"
    bash <(curl -s https://codecov.io/bash)
fi

# -------------------------------------------------------------------------------------------------
# INFRASTRUCTURE
# -------------------------------------------------------------------------------------------------
yarn --cwd ./infrastructure
yarn --cwd ./infrastructure build
