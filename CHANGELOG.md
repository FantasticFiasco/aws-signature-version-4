# Change Log

All notable changes to this project will be documented in this file.

This project adheres to [Semantic Versioning](http://semver.org/) and is following the [change log format](http://keepachangelog.com/).

## Unreleased

## [1.2.0] - 2019-07-15

### :zap: Added

- Improve searchability by adding NuGet tags `aws`, `sign`, `http` and `request`

## [1.1.0] - 2019-07-13

### :zap: Added

- [#36](https://github.com/FantasticFiasco/aws-signature-version-4/issues/36) Support for .NET Framework 4.5

## [1.0.2] - 2019-06-27

### :syringe: Fixed

- [#28](https://github.com/FantasticFiasco/aws-signature-version-4/issues/28) Default request headers are added twice on Android using Mono. The behavior on Mono is different from the behavior on .NET Framework or .NET Core, where a default request header that already exists on the request message is ignored. (contribution by [@Daniel-NP](https://github.com/Daniel-NP))

## [1.0.1] - 2019-05-28

### :syringe: Fixed

- Update XML comments
- Update dependencies

## [1.0.0] - 2019-05-28

Initial version.
