#!/bin/bash

echo -e "$(curl --silent https://raw.githubusercontent.com/FantasticFiasco/logo/master/logo.ansi)"
echo

echo "build: Build started"
echo "build: dotnet cli v$(dotnet --version)"
dotnet build -c Release

echo "test: Test started"
dotnet test -c Release
