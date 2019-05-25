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
- [Install via NuGet](#install-via-nuget)
- [Donations](#donations)
- [Credit](#credit)

## Introduction

TODO

This will be the birthplace for the buttoned up and boring, but deeply analyzed, implementation of Signature Version 4 in .NET.

Stay tuned for updates!


## Super simple to use

extensions to `HttpClient` `DeleteAsync` `GetAsync` `PostAsync` `PutAsync` `SendAsync`

TODO

```csharp
var client = new HttpClient();
var credentials = new ImmutableCredentials("<access key id>", "<secret access key>");

var response = await client.GetAsync(
    "https://www.acme.com/resources",
    "us-west-1",
    "execute-api",
    credentials);

```

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
