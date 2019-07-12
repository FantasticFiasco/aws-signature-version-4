# -------------------------------------------------------------------------------------------------
# LOGO
# -------------------------------------------------------------------------------------------------
$LOGO = (Invoke-WebRequest "https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.raw").toString();
Write-Host "$LOGO" -ForegroundColor Green

# -------------------------------------------------------------------------------------------------
# VARIABLES
# -------------------------------------------------------------------------------------------------
$GIT_SHA = "$env:APPVEYOR_REPO_COMMIT".substring(0, 7)
$IS_TAGGED_BUILD = If ("$env:APPVEYOR_REPO_TAG" -eq "true") { $true } Else { $false }
$IS_PULL_REQUEST = If ("$env:APPVEYOR_PULL_REQUEST_NUMBER" -eq "") { $false } Else { $true }
Write-Host "[info] git sha: $GIT_SHA"
Write-Host "[info] is git tag: $IS_TAGGED_BUILD"
Write-Host "[info] is pull request: $IS_PULL_REQUEST"
