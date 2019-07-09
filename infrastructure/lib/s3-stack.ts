// import { IIdentity } from '@aws-cdk/aws-iam';
// import { Bucket } from '@aws-cdk/aws-s3';
// import { CfnOutput, Construct, RemovalPolicy, Stack, StackProps } from '@aws-cdk/core';
// import { S3Item } from './s3-item';

// export interface S3StackProps extends StackProps {
//     readAccess: IIdentity[];
// }

// export class S3Stack extends Stack {
//     constructor(scope: Construct, id: string, props: S3StackProps) {
//         super(scope, id, props);

//         const bucket = new Bucket(this, 'Bucket', {
//             removalPolicy: RemovalPolicy.Destroy,
//         });

//         for (const identity of props.readAccess) {
//             bucket.grantRead(identity);
//         }

//         new S3Item(this, 'FooItem', {
//             bucket,
//             key: 'foo.txt',
//             value: 'This is foo',
//         });

//         new S3Item(this, 'BarItem', {
//             bucket,
//             key: 'foo/bar.txt',
//             value: 'This is bar',
//         });

//         new CfnOutput(this, 'BucketName', {
//             value: bucket.bucketName,
//         });

//         new CfnOutput(this, 'BucketArn', {
//             value: bucket.bucketArn,
//         });
//     }
// }
