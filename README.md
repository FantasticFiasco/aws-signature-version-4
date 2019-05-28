# AwsSignatureVersion4 - The buttoned up and boring, but deeply analyzed, implementation of SigV4 in .NET.

[![Build status](https://ci.appveyor.com/api/projects/status/bh71gd22ogf2ogvl/branch/master?svg=true)](https://ci.appveyor.com/project/FantasticFiasco/aws-signature-version-4)
[![codecov](https://codecov.io/gh/FantasticFiasco/aws-signature-version-4/branch/master/graph/badge.svg)](https://codecov.io/gh/FantasticFiasco/aws-signature-version-4)
[![NuGet Version](http://img.shields.io/nuget/v/AwsSignatureVersion4.svg?style=flat)](https://www.nuget.org/packages/AwsSignatureVersion4/)
[![NuGet](https://img.shields.io/nuget/dt/AwsSignatureVersion4.svg)](https://www.nuget.org/packages/AwsSignatureVersion4/)
[![License: Apache-2.0](https://img.shields.io/badge/license-Apache--2.0-blue.svg)](https://raw.githubusercontent.com/FantasticFiasco/aws-signature-version-4/master/LICENSE)

__Package__ - [AwsSignatureVersion4](https://www.nuget.org/packages/AwsSignatureVersion4)
| __Platforms__ - .NET Standard 2.0

## Table of contents

- [Introduction](#introduction)
- [Super simple to use](#super-simple-to-use)
- [The pledge](#the-pledge)
- [Install via NuGet](#install-via-nuget)
- [Donations](#donations)
- [Credit](#credit)

## Introduction

This project is unique for me. It's the first that isn't a labor of love.

Having to sign requests in AWS, I went through some emotions. My first was disappointment, directed at Amazon for not including a Signature Version 4 signer in their [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/). The functionality is listed on [Open Feature Requests for the AWS SDK for .NET](https://github.com/aws/aws-sdk-net/blob/master/FEATURE_REQUESTS.md) but I haven't seen any actions towards an implementation yet.

My second emotion was resentment. Not towards Amazon but towards developers proclaiming to have working implementations in their GitHub repositories, of which some had numerous stars and thousands of NuGet downloads. After analyzing their code and comparing it against the [steps of the signing algorithm](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html), I found them all to be blatantly lacking.

So here we are, yet another attempt at implementing the Signature Version 4 algorithm in .NET. Lets try to make sure that this one has fewer bugs than the previous attempts...

## Super simple to use

The best API is the one you already know. AwsSignatureVersion4 extends the class `HttpClient` by providing additional overloads to `DeleteAsync`, `GetAsync`, `PostAsync`, `PutAsync` and `SendAsync`. These overloads accept the following additional arguments.

- **Region name** - The name of the AWS region, e.g. `us-west-1`
- **Service name** - The name of the service, e.g. `execute-api` for an API Gateway
- **Credentials** - The AWS credentials of the principal sending the request

The extension methods integrate with `HttpClient` as one would expect, allowing you to set `HttpClient.BaseAddress` and `HttpClient.DefaultRequestHeaders`.

The following code is getting resources from an IAM authenticated API Gateway.

```csharp
var client = new HttpClient();
var credentials = new ImmutableCredentials("<access key id>", "<secret access key>", null);

var response = await client.GetAsync(
    "https://www.acme.com/resources",
    regionName: "us-west-1",
    serviceName: "execute-api",
    credentials: credentials);
```

For more examples, please see the [tests](https://github.com/FantasticFiasco/aws-signature-version-4/tree/master/test).

## The pledge

This project comes with a pledge, providing transparency on supported and unsupported scenarios.

- Amazon S3 (Amazon Simple Storage Service) is currently not supported. Please give [issue #1](https://github.com/FantasticFiasco/aws-signature-version-4/issues/1) a thumbs up if you wish it to be supported.
- No [steps of the signing algorithm](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html) have deliberately been left out
- About 170 unit tests are run before any release
- About 180 integration tests targeting an IAM authenticated API Gateway are run before any release
- Implementation is passing the [Signature Version 4 Test Suite](https://docs.aws.amazon.com/general/latest/gr/signature-v4-test-suite.html) scenarios, with the following exceptions:
    - `get-vanilla-query-unreserved` - This scenario defines a request URI that isn't supported by API Gateway
    - `get-utf8` - The signing algorithm states the following: *'Each path segment must be URI-encoded twice except for Amazon S3 which only gets URI-encoded once.'*. This scenario does not URL encode the path segments twice, only once.
    - `normalize-path/get-space` - The signing algorithm states the following: *'Each path segment must be URI-encoded twice except for Amazon S3 which only gets URI-encoded once.'*. This scenario does not URL encode the path segments twice, only once.
    - `post-sts-token/post-sts-header-before` - This scenario is based on the fact that the signing algorithm should support STS tokens, e.g. by assuming a role. This scenario is already covered by numerous other integration tests and can because of this safely be ignored.
    - `post-x-www-form-urlencoded` - The header `Content-Length` is specified in the canonical request file, but not in the authorization header file, nor the signed request file. AWS Technical Writers have been notified, and we are awaiting answer.
    - `post-x-www-form-urlencoded-parameters` - This scenario is based on the fact that we need to specify the charset in the `Content-Type` header, i.e. `Content-Type:application/x-www-form-urlencoded; charset=utf-8`. This is not necessary because .NET will add this encoding if omitted by us. We can safely skip this test and rely on integration tests where actual content is sent to an API Gateway.
- Implementation is reusing as much as possible from [AWSSDK.Core](https://www.nuget.org/packages/AWSSDK.Core/), thus the dependency
- Implementation is supporting authentication using the HTTP header `Authorization`
- Implementation is not supporting query string authentication
- Implementation has only been tested using HTTP/1.1

## Install via NuGet

If you want to include AwsSignatureVersion4 in your project, you can [install it directly from NuGet](https://www.nuget.org/packages/AwsSignatureVersion4/).

To install AwsSignatureVersion4, run the following command in the Package Manager Console:

```
PM> Install-Package AwsSignatureVersion4
```

## Donations

If this project has helped you to stay productive and save money, you can buy me a cup of coffee :)

[![PayPal Donate](https://img.shields.io/badge/Donate-PayPal-green.svg)](https://www.paypal.me/FantasticFiasco)

## Credit

Thank you [JetBrains](https://www.jetbrains.com/) for your important initiative to support the open source community with free licenses to your products.

![JetBrains](./doc/resources/jetbrains.png)
