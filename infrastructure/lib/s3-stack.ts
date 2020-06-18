import { IIdentity } from '@aws-cdk/aws-iam';
import { Bucket } from '@aws-cdk/aws-s3';
import { BucketDeployment, Source } from '@aws-cdk/aws-s3-deployment';
import { CfnOutput, Construct, RemovalPolicy, Stack, StackProps } from '@aws-cdk/core';

export interface S3StackProps extends StackProps {
    readWriteAccess: IIdentity[];
}

export class S3Stack extends Stack {
    constructor(scope: Construct, id: string, props: S3StackProps) {
        super(scope, id, props);

        const bucket = new Bucket(this, 'Bucket', {
            removalPolicy: RemovalPolicy.DESTROY,
        });

        for (const identity of props.readWriteAccess) {
            bucket.grantReadWrite(identity);
        }

        new BucketDeployment(this, 'BucketItems', {
            sources: [Source.asset('./s3')],
            destinationBucket: bucket,
        });

        new CfnOutput(this, 'BucketName', {
            value: bucket.bucketName,
        });

        new CfnOutput(this, 'BucketArn', {
            value: bucket.bucketArn,
        });
    }
}
