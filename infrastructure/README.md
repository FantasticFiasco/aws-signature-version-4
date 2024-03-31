# Infrastructure for integration tests

The infrastructure is provisioned using the [AWS CDK](https://docs.aws.amazon.com/cdk/api/latest/). The project is separated into three stacks:

- `SigV4-Users` - Creates the IAM users
- `SigV4-ApiGateway` - Creates an API Gateway with IAM authentication
- `SigV4-S3` - Creates the S3 bucket

These stacks are a prerequisite for running the integration tests.

Provision the infrastructure using the following commands.

```bash
npm install
npm run deploy
```
