import { AuthorizationType, LambdaRestApi } from '@aws-cdk/aws-apigateway';
import { DnsValidatedCertificate } from '@aws-cdk/aws-certificatemanager';
import { Code, Function, Runtime } from '@aws-cdk/aws-lambda';
import { HostedZoneProvider } from '@aws-cdk/aws-route53';
import { Construct, Stack, StackProps } from '@aws-cdk/cdk';

export class ApiGatewayStack extends Stack {
    constructor(scope: Construct, id: string, props?: StackProps) {
        super(scope, id, props);

        // Create Lambda
        const requestHandler = new Function(this, 'ApiRequestHandler', {
            code: Code.asset('resources'),
            handler: 'request.handler',
            runtime: Runtime.NodeJS10x,
        });

        // Create API Gateway
        const api = new LambdaRestApi(this, 'Api', {
            handler: requestHandler,
            options: {
                description: 'REST API endpoint for Signature Version 4 tests',
                restApiName: 'SignatureVersion4',
            },
            proxy: false,
        });

        api.root.addMethod('ANY', undefined, {
            authorizationType: AuthorizationType.IAM,
        });

        api.root.addResource('{proxy+}').addMethod('ANY', undefined, {
            authorizationType: AuthorizationType.IAM,
        });

        // Get the existing hosted zone
        const hostedZone = new HostedZoneProvider(this, {
            domainName: 'fantasticfiasco.com',
        }).findAndImport(this, 'HostedZone');

       // Create certificate
        new DnsValidatedCertificate(this, 'Certificate', {
            domainName: 'www.sigv4.fantasticfiasco.com',
            hostedZone,
        });

        // TODO: The following steps are manual due do lacking support in the AWS SDK.
        //
        // Create a custom domain name:
        //   1. Navigate to API Gateway in the console
        //   2. Select 'Custom Domain Names'
        //   3. Click 'Create Custom Domain Name'
        //   4. Create custom domain name with the following arguments:
        //     - HTTP or WebSocket protocol: HTTP
        //     - Domain name: www.sigv4.fantasticfiasco.com
        //     - Security policy: TLS 1.2
        //     - Endpoint configuration: Regional
        //     - ACM Certificate: <created certificate>
        //   5. Add a base path mapping with the following arguments:
        //     - Path: '/'
        //     - Destination: <SignatureVersion4>
        //     - Stage: <prod>
        //
        // Create DNS record
        //   1. Log into The AWS Console
        //   2. Navigate to Route 53 and create a new record set with the following arguments:
        //     - Name: www.sigv4.fantasticfiasco.com
        //     - Type: A - IPv4 Address
        //     - Alias: Yes
        //     - Alias Target: <target domain name>
        //     - Routing policy: Simple
        //     - Evaluate Target Health: No
        //   3. Wait for about  60 seconds, it may take that long for the changes to be applied
    }
}
