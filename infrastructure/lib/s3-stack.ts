import { IIdentity } from '@aws-cdk/aws-iam';
import { Bucket } from '@aws-cdk/aws-s3';
import { CfnOutput, Construct, Stack, StackProps } from '@aws-cdk/cdk';

export interface S3StackProps extends StackProps {
    readAccess: IIdentity[];
}

export class S3Stack extends Stack {
    constructor(scope: Construct, id: string, props: S3StackProps) {
        super(scope, id, props);

        const bucket = new Bucket(this, 'Bucket');

        for (const identity of props.readAccess) {
            bucket.grantRead(identity);
        }

        new CfnOutput(this, 'BucketName', {
            value: bucket.bucketName,
        });

        new CfnOutput(this, 'BucketArn', {
            value: bucket.bucketArn,
        });
    }
}
