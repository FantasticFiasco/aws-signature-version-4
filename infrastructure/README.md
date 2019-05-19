# Infrastructure for integration tests

The infrastructure is provisioned using the [AWS CDK](https://docs.aws.amazon.com/cdk/api/latest/). The project is separated into two stacks, `Sigv4-Users` and `Sigv4-IamAuthentication`. The former is creating IAM users and the latter is creating a API Gateway with IAM authentication. These stacks are a prerequisite for running the integration tests.

Provision the infrastructure using the following commands.

```bash
yarn
yarn deploy
```
