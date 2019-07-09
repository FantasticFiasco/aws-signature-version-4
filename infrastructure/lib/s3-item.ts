// import { AwsCustomResource } from '@aws-cdk/aws-cloudformation';
// import { IBucket } from '@aws-cdk/aws-s3';
// import { Construct } from '@aws-cdk/core';

// export interface S3ItemProps {
//     bucket: IBucket;
//     key: string;
//     value: string;
// }

// export class S3Item extends Construct {
//     constructor(scope: Construct, id: string, props: S3ItemProps) {
//         super(scope, id);

//         new AwsCustomResource(this, 'PutObject', {
//             onDelete: {
//                 action: 'deleteObject',
//                 parameters: {
//                     Bucket: props.bucket.bucketName,
//                     Key: props.key,
//                 },
//                 // Update physical id to always put the latest version
//                 physicalResourceId: Date.now().toString(),
//                 service: 'S3',
//             },
//             // Update is also called when resource is created
//             onUpdate: {
//                 action: 'putObject',
//                 parameters: {
//                     Body: props.value,
//                     Bucket: props.bucket.bucketName,
//                     Key: props.key,
//                 },
//                 // Update physical id to always put the latest version
//                 physicalResourceId: Date.now().toString(),
//                 service: 'S3',
//             },
//         });
//     }
// }
