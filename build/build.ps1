# -------------------------------------------------------------------------------------------------
# LOGO
# -------------------------------------------------------------------------------------------------
$LOGO = (Invoke-WebRequest "https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.raw").toString();
Write-Host "$LOGO" -ForegroundColor Green

# -------------------------------------------------------------------------------------------------
# VARIABLES
# -------------------------------------------------------------------------------------------------
$GIT_SHA = "$env:APPVEYOR_REPO_COMMIT".substring(0, 7)
Write-Host "$GIT_SHA"



# GIT_SHA="${APPVEYOR_REPO_COMMIT:0:7}"
# [ "${APPVEYOR_REPO_TAG}" = "true" ] && IS_TAGGED_BUILD=true || IS_TAGGED_BUILD=false
# [ ! -z "${APPVEYOR_PULL_REQUEST_NUMBER}" ] && IS_PULL_REQUEST=true || IS_PULL_REQUEST=false
# echo "[info] git sha: ${GIT_SHA}"
# echo "[info] is git tag: ${IS_TAGGED_BUILD}"
# echo "[info] is pull request: ${IS_PULL_REQUEST}"
