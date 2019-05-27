# AwsSignatureVersion4 - The serious SigV4 implementation in .NET

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

TODO

This will be the birthplace for the buttoned up and boring, but deeply analyzed, implementation of Signature Version 4 in .NET.

Stay tuned for updates!


## Super simple to use

The most intuitive API is the one you already know. AwsSignatureVersion4 extends `HttpClient` by providing additional overloads to `HttpClient.DeleteAsync`, `HttpClient.GetAsync`, `HttpClient.PostAsync`, `HttpClient.PutAsync` and `HttpClient.SendAsync`. The overloads accept the following additional arguments.

- **Region name** - The name of the AWS region, e.g. `us-west-1`
- **Service name** - The name of the service, e.g. `execute-api` for an API Gateway
- **Credentials** - The AWS credentials of the entity sending the request

The following example is getting resources from an IAM authenticated API Gateway.

```csharp
var client = new HttpClient();
var credentials = new ImmutableCredentials("<access key id>", "<secret access key>", null);

var response = await client.GetAsync(
    "https://www.acme.com/resources",
    regionName: "us-west-1",
    serviceName: "execute-api",
    credentials: credentials);
```

The other overloads follow the same pattern, and are really easy to use!

## The pledge

### Exemption clause

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


BIN

# AWS Signature Version 4

This is the first time I've written open source that is not a labor of love. It is code fueled by anger at AWS for not providing signing functionality in the AWS SDK, and for resentment at those other developers in GitHub Gists or projects that say they provide a signing algorithm, when one clearly can see that always only implement parts of the algorithms. I mean, who doesn't enjoy debugging requests in production that sometimes fail? That was irony, and I hope the developers have felt the same fear when their dashboard of their production site is red due to their own mistakes.

The pledge

What's not part of the deal

## Disclosure

- only been tested on HTTP/1.1

- Test suite
A note about signing requests to Amazon S3:

In exception to this, you do not normalize URI paths for requests to Amazon S3. For example, if you have a bucket with an object named my-object//example//photo.user, use that path. Normalizing the path to my-object/example/photo.user will cause the request to fail. For more information, see Task 1: Create a Canonical Request in the Amazon Simple Storage Service API Reference: http://docs.aws.amazon.com/AmazonS3/latest/API/sig-v4-header-based-auth.html#canonical-request

Easier if I say that I dont support s3, or write tests for it I guess

- Authenticating Requests: Using Query Parameters https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-query-string-auth.html

Does only support HTTP header Authorization, not query string authorization (https://docs.aws.amazon.com/general/latest/gr/sigv4-add-signature-to-request.html)

When you add the X-Amz-Security-Token parameter to the query string, some services require that you include this parameter in the canonical (signed) request. For other services, you add this parameter at the end, after you calculate the signature. For details, see the API reference documentation for that service.

Not supporting funny UTF8 charcters like the scenario get-utf8
GET /ሴ HTTP/1.1
Host:example.amazonaws.com
X-Amz-Date:20150830T123600Z


