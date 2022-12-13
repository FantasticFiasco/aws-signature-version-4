<h1 align="center">
  AwsSignatureVersion4
  <br>
</h1>

<h4 align="center">The buttoned-up and boring, but deeply analyzed, implementation of Signature Version 4 (SigV4) in .NET.</h4>

<p align="center">
  <a href="https://ci.appveyor.com/project/FantasticFiasco/aws-signature-version-4"><img src="https://ci.appveyor.com/api/projects/status/96upkt8x02mhqi5b/branch/master?svg=true"></a>
  <a href="https://codecov.io/gh/FantasticFiasco/aws-signature-version-4"><img src="https://codecov.io/gh/FantasticFiasco/aws-signature-version-4/branch/master/graph/badge.svg"></a>
  <a href="https://www.nuget.org/packages/AwsSignatureVersion4/"><img src="http://img.shields.io/nuget/v/AwsSignatureVersion4.svg?style=flat"></a>
  <a href="https://semver.org/"><img src="https://img.shields.io/badge/%E2%9C%85-SemVer%20compatible-blue"></a>
  <a href="https://www.nuget.org/packages/AwsSignatureVersion4/"><img src="https://img.shields.io/nuget/dt/AwsSignatureVersion4.svg"></a>
  <a href="https://raw.githubusercontent.com/FantasticFiasco/aws-signature-version-4/master/LICENSE"><img src="https://img.shields.io/badge/license-Apache--2.0-blue.svg"></a>
</p>

<p align="center">
  <b>Package</b> - <a href="https://www.nuget.org/packages/AwsSignatureVersion4">AwsSignatureVersion4</a><br>
  <b>Platforms</b> - .NET Standard 2.0, .NET 6
</p>

## Table of contents <!-- omit in toc -->

- [Introduction](#introduction)
- [Super simple to use](#super-simple-to-use)
  - [Integration with `HttpClient`](#integration-with-httpclient)
  - [Integration with `IHttpClientFactory`](#integration-with-ihttpclientfactory)
- [Credentials](#credentials)
- [The pledge](#the-pledge)
- [Install via NuGet](#install-via-nuget)
- [Credit](#credit)

## Introduction

This project is unique for me. It's my first that isn't a labor of love.

Having to sign requests in AWS I went through a series of emotions. My first was disappointment, directed at Amazon for not including a Signature Version 4 signer in their [AWS SDK for .NET](https://aws.amazon.com/sdk-for-net/). The functionality is listed on [Open Feature Requests for the AWS SDK for .NET](https://github.com/aws/aws-sdk-net/blob/master/FEATURE_REQUESTS.md) but I haven't seen any actions towards an implementation yet.

My second emotion was being overwhelmed. The signing algorithm involved [many more steps](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html) than I'd thought be possible, and I knew I'd have to spend a lot of time getting conformable with the algorithm.

So here we are, my attempt at implementing the Signature Version 4 algorithm in .NET. Please lets hope that AWS SDK soon releases this functionality so we can deprecate this piece of code...

## Super simple to use

The best API is the one you already know. By extending both `HttpClient` and `IHttpClientFactory` we hope you'll get an easy integration.

### Integration with `HttpClient`

This project extends the class `HttpClient` by providing additional overloads to `DeleteAsync`, `GetAsync`, `GetStringAsync`, `PatchAsync`, `PostAsync`, `PutAsync`, `SendAsync`, and the new synchronous addition to .NET 5 and onwards, `Send`. These overloads accept the following additional arguments.

- `regionName` - The name of the AWS region, e.g. `us-west-1`
- `serviceName` - The name of the service, e.g. `execute-api` for an AWS API Gateway
- `credentials` - The AWS credentials of the principal sending the request

These overloads are built to integrate with `HttpClient`, i.e. `HttpClient.BaseAddress` and `HttpClient.DefaultRequestHeaders` will be respected when sending the request.

The following example is demonstrating how one would send a `GET /resources` request to an IAM authenticated AWS API Gateway on host `www.acme.com`.

```csharp
// Don't specify credentials in source code, this is for demo only! See next chapter for more
// information.
var credentials = new ImmutableCredentials("<access key id>", "<secret access key>", null);

var client = new HttpClient();
var response = await client.GetAsync(
  "https://www.acme.com/resources",
  regionName: "us-west-1",
  serviceName: "execute-api",
  credentials: credentials);
```

Please see the [tests](https://github.com/FantasticFiasco/aws-signature-version-4/tree/master/test) directory for other examples.

### Integration with `IHttpClientFactory`

This project supports `IHttpClientFactory` by means of providing a custom HTTP message handler called `AwsSignatureHandler`. Once injected into the pipeline of the HTTP client factory it will sign your requests before sending them over the network.

`AwsSignatureHandler` takes an instance of `AwsSignatureHandlerSettings` as its only constructor argument, thus you will have to configure your dependency injection container to sufficiently resolve both these classes.

The following example is demonstrating how one would configure the ASP .NET Core service collection to acknowledge a HTTP client named `my-client`. For more information regarding configuration, please see [Dependency injection in ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/dependency-injection).

```csharp
// Don't specify credentials in source code, this is for demo only! See next chapter for more
// information.
var credentials = new ImmutableCredentials("<access key id>", "<secret access key>", null);
services
  .AddTransient<AwsSignatureHandler>()
  .AddTransient(_ => new AwsSignatureHandlerSettings("us-west-1", "execute-api", credentials));

services
  .AddHttpClient("my-client")
  .AddHttpMessageHandler<AwsSignatureHandler>();
```

Please see the [tests](https://github.com/FantasticFiasco/aws-signature-version-4/tree/master/test) directory for other examples.

## Credentials

We've come a long way, but let's back up a step. Credentials should not be specified in source code, so where do they come from?

It all starts with a [principal](https://docs.aws.amazon.com/IAM/latest/UserGuide/intro-structure.html#intro-structure-principal), i.e. an entity identifying itself using authentication. In some situations the principal is a [IAM user](https://docs.aws.amazon.com/IAM/latest/UserGuide/id_users.html) and in other situations it is an entity assuming a [IAM role](https://docs.aws.amazon.com/IAM/latest/UserGuide/id_roles.html). Whatever your principal is, it has the capability of providing credentials.

How the credentials are provided depend on where you run your code. If you run your code in a ECS Task you get your credentials using [ECSTaskCredentials](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/items/Runtime/TECSTaskCredentials.html). Other runtime will require other credential providers, all of them are listed in the namespace [Amazon.Runtime](https://docs.aws.amazon.com/sdkfornet/v3/apidocs/index.html?page=Runtime/NRuntime.html&tocid=Amazon_Runtime).

## The pledge

This project comes with a pledge, providing transparency on supported and unsupported scenarios.

- :white_check_mark: ~200 unit tests are passing before a release
- :white_check_mark: ~700 integration tests targeting an IAM authenticated AWS API Gateway are passing before a release
- :white_check_mark: ~300 integration tests targeting an IAM authenticated AWS S3 bucket are passing before a release
- :white_check_mark: No [steps of the signing algorithm](https://docs.aws.amazon.com/general/latest/gr/sigv4_signing.html) have deliberately been left out
- :white_check_mark: [AWSSDK.Core](https://www.nuget.org/packages/AWSSDK.Core/) is reused as much as possible, thus the dependency
- :white_check_mark: [Signature Version 4 Test Suite](https://docs.aws.amazon.com/general/latest/gr/signature-v4-test-suite.html) scenarios are passing, with the following exceptions:
  - General
    - :x: `get-utf8` - The signing algorithm states the following: _'Each path segment must be URI-encoded twice except for Amazon S3 which only gets URI-encoded once.'_. This scenario does not URL encode the path segments twice, only once.
    - :x: `normalize-path/get-space` - The signing algorithm states the following: _'Each path segment must be URI-encoded twice except for Amazon S3 which only gets URI-encoded once.'_. This scenario does not URL encode the path segments twice, only once.
    - :x: `post-x-www-form-urlencoded` - This scenario is based on the fact that we need to specify the charset in the `Content-Type` header, e.g. `Content-Type:application/x-www-form-urlencoded; charset=utf-8`. This is not necessary because .NET will add this encoding if omitted by us. We can safely skip this test and rely on integration tests where actual content is sent to an AWS API Gateway.
    - :x: `post-x-www-form-urlencoded-parameters` - This scenario is based on the fact that we need to specify the charset in the `Content-Type` header, e.g. `Content-Type:application/x-www-form-urlencoded; charset=utf-8`. This is not necessary because .NET will add this encoding if omitted by us. We can safely skip this test and rely on integration tests where actual content is sent to an AWS API Gateway.
  - API Gateway
    - :x: `get-vanilla-query-unreserved` - This scenario defines a request that isn't supported by AWS API Gateway
    - :x: `post-sts-token/post-sts-header-before` - This scenario is based on the fact that the signing algorithm should support STS tokens, e.g. by assuming a role. This scenario is already covered by numerous other integration tests and can because of this safely be ignored.
  - S3
    - :x: `normalize-path/get-slash-pointless-dot` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-header-key-case` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-header-key-sort` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-header-value-case` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-sts-token/post-sts-header-after` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-sts-token/post-sts-header-before` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-vanilla` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-vanilla-empty-query-value` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-vanilla-query` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-x-www-form-urlencoded` - This scenario defines a request that isn't supported by AWS S3.
    - :x: `post-x-www-form-urlencoded-parameters` - This scenario defines a request that isn't supported by AWS S3.
- :white_check_mark: All characters are supported in S3 object keys with the following exceptions:
  - :x: Plus (`+`)
  - :x: Backslash (`\`)
  - :x: Left curly brace (`{`)
  - :x: Right curly brace (`}`)
  - :x: Left square bracket (`[`)
  - :x: Right square bracket (`]`)
  - :x: 'Less Than' symbol (`<`)
  - :x: 'Greater Than' symbol (`>`)
  - :x: Grave accent / back tick (`` ` ``)
  - :x: 'Pound' character (`#`)
  - :x: Caret (`^`)
  - :x: Percent character (`%`)
  - :x: Tilde (`~`)
  - :x: Vertical bar / pipe (`|`)
  - :x: Non-printable ASCII characters (128–255 decimal characters)
  - :x: Quotation marks
- Authentication method
  - :white_check_mark: HTTP header authentication is supported
  - :x: Query string authentication is not supported
- HTTP version
  - :white_check_mark: HTTP/1.1 is supported
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

## Credit

Thank you [JetBrains](https://www.jetbrains.com/) for your important initiative to support the open source community with free licenses to your products.

![JetBrains](./doc/resources/jetbrains.png)
