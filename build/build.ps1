function Print {
    param (
        $Object
    )

    Write-Host "$Object" -ForegroundColor Green
}

# -------------------------------------------------------------------------------------------------
# LOGO
# -------------------------------------------------------------------------------------------------
$logo = (Invoke-WebRequest "https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.raw").toString();
Print "$logo"

# -------------------------------------------------------------------------------------------------
# VARIABLES
# -------------------------------------------------------------------------------------------------
$git_sha = "$env:APPVEYOR_REPO_COMMIT".substring(0, 7)
$is_tagged_build = If ("$env:APPVEYOR_REPO_TAG" -eq "true") { $true } Else { $false }
$is_pull_request = If ("$env:APPVEYOR_PULL_REQUEST_NUMBER" -eq "") { $false } Else { $true }
Write-Host "[info] git sha: $git_sha"
Write-Host "[info] is git tag: $is_tagged_build"
Write-Host "[info] is pull request: $is_pull_request"

# -------------------------------------------------------------------------------------------------
# BUILD
# -------------------------------------------------------------------------------------------------
Write-Host "[build] build started"
Write-Host "[build] dotnet cli v$(dotnet --version)"
$version_suffix_arg = If ($is_tagged_build -eq $true) { "" } Else { "--version-suffix=sha-$git_sha" }
dotnet build -c Release $version_suffix_arg
if ($LASTEXITCODE -ne 0) { exit 1 }
dotnet pack -c Release -o ./artifacts --no-build $version_suffix_arg
if ($LASTEXITCODE -ne 0) { exit 1 }

# -------------------------------------------------------------------------------------------------
# TEST
# -------------------------------------------------------------------------------------------------
Write-Host "[test] test started"

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

        Write-Host "[test] upload coverage report from $testResult"
        Invoke-WebRequest -Uri "https://codecov.io/bash" -OutFile codecov.sh
        bash codecov.sh -f "coverage.cobertura.xml"
        if ($LASTEXITCODE -ne 0) { exit 1 }

        Pop-Location
    }
}

# -------------------------------------------------------------------------------------------------
# INFRASTRUCTURE
# -------------------------------------------------------------------------------------------------
Write-Host "[infrastructure] build started"
Write-Host "[infrastructure] node $(node --version)"
yarn --cwd ./infrastructure
yarn --cwd ./infrastructure build
