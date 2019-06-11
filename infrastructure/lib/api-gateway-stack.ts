import { AuthorizationType, CfnDomainNameV2, LambdaRestApi } from '@aws-cdk/aws-apigateway';
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

        // Create Route53 records
        const hostedZone = new HostedZoneProvider(this, {
            domainName: 'fantasticfiasco.com',
        }).findAndImport(this, 'HostedZone');

        // Create certificate
        const certificate = new DnsValidatedCertificate(this, 'Certificate', {
            domainName: 'sigv4.fantasticfiasco.com',
            hostedZone,
        });

        // Create custom domain name
        new CfnDomainNameV2(this, 'ApiCustomDomainName', {
            domainName: 'sigv4.fantasticfiasco.com',
            domainNameConfigurations: [
                {
                    certificateArn: certificate.certificateArn,
                    endpointType: 'REGIONAL',
                },
            ],
        });

        // const target = AddressRecordTarget.fromAlias({});

        // new ARecord(this, 'ARecord', {
        //     recordName: 'sigv4.fantasticfiasco.com',
        //     target: AddressRecordTarget.fromAlias(new Alias)
        //     zone: hostedZone,
        // });

    }
}
