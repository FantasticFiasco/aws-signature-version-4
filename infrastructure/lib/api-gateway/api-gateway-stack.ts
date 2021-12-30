import { CfnOutput, Stack, StackProps } from 'aws-cdk-lib'
import { AuthorizationType, LambdaRestApi } from 'aws-cdk-lib/aws-apigateway'
import { Code, Function, Runtime } from 'aws-cdk-lib/aws-lambda'
import { Construct } from 'constructs'

export class ApiGatewayStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props)

    // Create Lambda
    const requestHandler = new Function(this, 'ApiRequestHandler', {
      code: Code.fromAsset(`${__dirname}/handlers`),
      handler: 'request.handler',
      runtime: Runtime.NODEJS_14_X,
    })

    // Create API Gateway
    const api = new LambdaRestApi(this, 'Api', {
      handler: requestHandler,
      description: 'REST API endpoint for Signature Version 4 tests',
      restApiName: 'SignatureVersion4',
      proxy: false,
    })

    api.root.addMethod('ANY', undefined, {
      authorizationType: AuthorizationType.IAM,
    })

    api.root.addResource('{proxy+}').addMethod('ANY', undefined, {
      authorizationType: AuthorizationType.IAM,
    })

    new CfnOutput(this, 'ApiGatewayUrl', {
      value: api.url.endsWith('/')
        ? api.url.slice(0, -1) // Remove the trailing slash
        : api.url,
    })
  }
}
