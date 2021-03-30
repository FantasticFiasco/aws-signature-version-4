import { AuthorizationType, LambdaRestApi } from '@aws-cdk/aws-apigateway'
import { Code, Function, Runtime } from '@aws-cdk/aws-lambda'
import { CfnOutput, Construct, Stack, StackProps } from '@aws-cdk/core'

export class ApiGatewayStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props)

    // Create Lambda
    const requestHandler = new Function(this, 'ApiRequestHandler', {
      code: Code.asset(`${__dirname}/handlers`),
      handler: 'request.handler',
      runtime: Runtime.NODEJS_10_X,
    })

    // Create API Gateway
    const api = new LambdaRestApi(this, 'Api', {
      handler: requestHandler,
      options: {
        description: 'REST API endpoint for Signature Version 4 tests',
        restApiName: 'SignatureVersion4',
      },
      proxy: false,
    })

    api.root.addMethod('ANY', undefined, {
      authorizationType: AuthorizationType.IAM,
    })

    api.root.addResource('{proxy+}').addMethod('ANY', undefined, {
      authorizationType: AuthorizationType.IAM,
    })

    new CfnOutput(this, 'ApiGatewayUrl', {
      value: api.url.slice(0, -1), // Remove the trailing slash
    })
  }
}
