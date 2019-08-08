# AwsSignatureVersion4 - The buttoned up and boring, but deeply analyzed, implementation of Signature Version 4 (SigV4) in .NET.

[![Build status](https://ci.appveyor.com/api/projects/status/96upkt8x02mhqi5b/branch/master?svg=true)](https://ci.appveyor.com/project/FantasticFiasco/aws-signature-version-4)
[![codecov](https://codecov.io/gh/FantasticFiasco/aws-signature-version-4/branch/master/graph/badge.svg)](https://codecov.io/gh/FantasticFiasco/aws-signature-version-4)
[![NuGet Version](http://img.shields.io/nuget/v/AwsSignatureVersion4.svg?style=flat)](https://www.nuget.org/packages/AwsSignatureVersion4/)
[![SemVer compatible](https://img.shields.io/badge/%E2%9C%85-SemVer%20compatible-blue)](https://semver.org/)
[![NuGet](https://img.shields.io/nuget/dt/AwsSignatureVersion4.svg)](https://www.nuget.org/packages/AwsSignatureVersion4/)
[![License: Apache-2.0](https://img.shields.io/badge/license-Apache--2.0-blue.svg)](https://raw.githubusercontent.com/FantasticFiasco/aws-signature-version-4/master/LICENSE)

__Package__ - [AwsSignatureVersion4](https://www.nuget.org/packages/AwsSignatureVersion4)
| __Platforms__ - .NET Framework 4.5, .NET Standard 2.0

## Table of contents

- [Introduction](#introduction)
- [Super simple to use](#super-simple-to-use)
- [The pledge](#the-pledge)
- [Install via NuGet](#install-via-nuget)
- [Donations](#donations)
- [Credit](#credit)

## Introduction

This project is unique for me. It's my first that isn't a labor of love.

Having to sign requests in AWS I went through a series of emotions. My first was disappointment, directed at Amazon for not including a Signature Version 4 signer in their [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/). The functionality is listed on [Open Feature Requests for the AWS SDK for .NET](https://github.com/aws/aws-sdk-net/blob/master/FEATURE_REQUESTS.md) but I haven't seen any actions towards an implementation yet.

My second emotion was being overwhelmed. The signing algorithm involved [many more steps](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html) than I'd thought be possible, and I knew I'd have to spend a lot of time getting conformable with the algorithm.

So here we are, my attempt at implementing the Signature Version 4 algorithm in .NET. Please lets hope that AWS SDK soon will release this functionality so we can deprecate this piece of code...

## Super simple to use

The best API is the one you already know. This project extends the class `HttpClient` by providing additional overloads to `PostAsync`, `GetAsync`, `PutAsync`, `DeleteAsync` and `SendAsync`. These overloads accept the following additional arguments.

- `regionName` - The name of the AWS region, e.g. `us-west-1`
- `serviceName` - The name of the service, e.g. `execute-api` for an AWS API Gateway
- `credentials` - The AWS credentials of the principal sending the request

These overloads are built to integrate with `HttpClient`, i.e. `HttpClient.BaseAddress` and `HttpClient.DefaultRequestHeaders` will be respected when sending the request.

The following example is demonstrating how one would send a `GET /resources` request to an IAM authenticated AWS API Gateway on host `www.acme.com`.

```csharp
var client = new HttpClient();
var credentials = new ImmutableCredentials("<access key id>", "<secret access key>", null);

var response = await client.GetAsync(
    "https://www.acme.com/resources",
    regionName: "us-west-1",
    serviceName: "execute-api",
    credentials: credentials);
```

Please see the [tests](https://github.com/FantasticFiasco/aws-signature-version-4/tree/master/test) directory for other examples.

## The pledge

This project comes with a pledge, providing transparency on supported and unsupported scenarios.

- :heavy_check_mark: Over 170 unit tests are passing before a release
- :heavy_check_mark: Over 180 integration tests targeting an IAM authenticated AWS API Gateway are passing before a release
- :heavy_check_mark: No [steps of the signing algorithm](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html) have deliberately been left out
- :heavy_check_mark: [AWSSDK.Core](https://www.nuget.org/packages/AWSSDK.Core/) is reused as much as possible, thus the dependency
- :heavy_check_mark: [Signature Version 4 Test Suite](https://docs.aws.amazon.com/general/latest/gr/signature-v4-test-suite.html) scenarios are passing, with the following exceptions:
  - :x: `get-vanilla-query-unreserved` - This scenario defines a request URI that isn't supported by AWS API Gateway
  - :x: `get-utf8` - The signing algorithm states the following: *'Each path segment must be URI-encoded twice except for Amazon S3 which only gets URI-encoded once.'*. This scenario does not URL encode the path segments twice, only once.
  - :x: `normalize-path/get-space` - The signing algorithm states the following: *'Each path segment must be URI-encoded twice except for Amazon S3 which only gets URI-encoded once.'*. This scenario does not URL encode the path segments twice, only once.
  - :x: `post-sts-token/post-sts-header-before` - This scenario is based on the fact that the signing algorithm should support STS tokens, e.g. by assuming a role. This scenario is already covered by numerous other integration tests and can because of this safely be ignored.
  - :x: `post-x-www-form-urlencoded` - The header `Content-Length` is specified in the canonical request file, but not in the authorization header file, nor the signed request file. AWS Technical Writers have been notified, and we are awaiting answer.
  - :x: `post-x-www-form-urlencoded-parameters` - This scenario is based on the fact that we need to specify the charset in the `Content-Type` header, i.e. `Content-Type:application/x-www-form-urlencoded; charset=utf-8`. This is not necessary because .NET will add this encoding if omitted by us. We can safely skip this test and rely on integration tests where actual content is sent to an AWS API Gateway.
- :x: Amazon S3 (Simple Storage Service) is currently not supported. Please give [issue #1](https://github.com/FantasticFiasco/aws-signature-version-4/issues/1) a thumbs up if you wish it to be supported.
- Authentication method
    - :heavy_check_mark: HTTP header authentication is supported
    - :x: Query string authentication is not supported
- HTTP version
    - :heavy_check_mark: HTTP/1.1 is supported
    - :x: HTTP/2 is not supported, please create an issue if you wish it to be supported

## Install via NuGet

If you want to include AwsSignatureVersion4 in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/AwsSignatureVersion4/).

To install AwsSignatureVersion4, run the following command in the Package Manager Console.

```
PM> Install-Package AwsSignatureVersion4
```

You can also install AwsSignatureVersion4 using the `dotnet` command line interface.

```bash
dotnet add package AwsSignatureVersion4
```


## Donations

If this project has helped you to stay productive and save money, you can buy me a cup of coffee :)

[![PayPal Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/FantasticFiasco)

## Credit

Thank you [JetBrains](https://www.jetbrains.com/) for your important initiative to support the open source community with free licenses to your products.

![JetBrains](./doc/resources/jetbrains.png)
