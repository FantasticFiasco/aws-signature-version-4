import { IRole, IUser } from '@aws-cdk/aws-iam';
import { Bucket } from '@aws-cdk/aws-s3';
import { CfnOutput, Construct, Stack, StackProps } from '@aws-cdk/cdk';

export interface S3StackProps extends StackProps {
    userWithReadAccess: IUser;
    roleWithReadAccess: IRole;
}

export class S3Stack extends Stack {
    constructor(scope: Construct, id: string, props: S3StackProps) {
        super(scope, id, props);

        const bucket = new Bucket(this, 'Bucket', {
        });

        bucket.grantRead(props.userWithReadAccess);
        bucket.grantRead(props.roleWithReadAccess);

        new CfnOutput(this, 'BucketName', {
            value: bucket.bucketName,
        });

        new CfnOutput(this, 'BucketArn', {
            value: bucket.bucketArn,
        });
    }
}
