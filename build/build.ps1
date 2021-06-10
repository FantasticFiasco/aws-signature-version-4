# -------------------------------------------------------------------------------------------------
# COMMON FUNCTIONS
# -------------------------------------------------------------------------------------------------
function Print {
    param (
        [string]$Category,
        [string]$Message
    )

    if ($Category) {
        Write-Host "[$Category] $Message" -ForegroundColor Green
    } else {
        Write-Host "$Message" -ForegroundColor Green
    }
}

# -------------------------------------------------------------------------------------------------
# LOGO
# -------------------------------------------------------------------------------------------------
$logo = (Invoke-WebRequest "https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.raw").toString();
Print -Message $logo

# -------------------------------------------------------------------------------------------------
# VARIABLES
# -------------------------------------------------------------------------------------------------
$git_sha = "$env:APPVEYOR_REPO_COMMIT".substring(0, 7)
$is_tagged_build = If ("$env:APPVEYOR_REPO_TAG" -eq "true") { $true } Else { $false }
$is_pull_request = If ("$env:APPVEYOR_PULL_REQUEST_NUMBER" -eq "") { $false } Else { $true }
Print "info" "git sha: $git_sha"
Print "info" "is git tag: $is_tagged_build"
Print "info" "is pull request: $is_pull_request"

# -------------------------------------------------------------------------------------------------
# BUILD
# -------------------------------------------------------------------------------------------------
Print "build" "build started"
Print "build" "dotnet cli v$(dotnet --version)"
$version_suffix_arg = If ($is_tagged_build -eq $true) { "" } Else { "--version-suffix=sha-$git_sha" }
dotnet build -c Release $version_suffix_arg
if ($LASTEXITCODE -ne 0) { exit 1 }
dotnet pack -c Release -o ./artifacts --no-build $version_suffix_arg
if ($LASTEXITCODE -ne 0) { exit 1 }

# -------------------------------------------------------------------------------------------------
# TEST
# -------------------------------------------------------------------------------------------------
Print "test" "test started"

if ($is_pull_request -eq $true) {
    # Exclude integration tests if we run as part of a pull requests. Integration tests rely on
    # secrets, which are omitted by AppVeyor on pull requests.
    dotnet test -c Release --no-build --filter Category!=Integration
    if ($LASTEXITCODE -ne 0) { exit 1 }
}
else {
    dotnet test -c Release --no-build --collect:"XPlat Code Coverage"
    if ($LASTEXITCODE -ne 0) { exit 1 }

    foreach ($testResult in Get-ChildItem .\test\TestResults\*)
    {
        Push-Location $testResult

        Print "test" "upload coverage report from $testResult"
        Invoke-WebRequest -Uri "https://uploader.codecov.io/latest/codecov.exe" -Outfile codecov.exe
        codecov.exe -f "coverage.cobertura.xml"
        if ($LASTEXITCODE -ne 0) { exit 1 }

        Pop-Location
    }
}

# -------------------------------------------------------------------------------------------------
# INFRASTRUCTURE
# -------------------------------------------------------------------------------------------------
Print "infrastructure" "build started"
Print "infrastructure" "node $(node --version)"
yarn --cwd ./infrastructure
yarn --cwd ./infrastructure build
