import { CfnOutput, Duration, RemovalPolicy, Stack, StackProps } from 'aws-cdk-lib'
import { IIdentity } from 'aws-cdk-lib/aws-iam'
import { Bucket } from 'aws-cdk-lib/aws-s3'
import { Construct } from 'constructs'

export interface S3StackProps extends StackProps {
  readWriteAccess: IIdentity[]
}

export class S3Stack extends Stack {
  constructor(scope: Construct, id: string, props: S3StackProps) {
    super(scope, id, props)

    const bucket = new Bucket(this, 'Bucket', {
      removalPolicy: RemovalPolicy.DESTROY,
    })

    bucket.addLifecycleRule({
      id: 'Delete temp',
      prefix: 'temp/',
      expiration: Duration.days(31),
    })

    for (const identity of props.readWriteAccess) {
      bucket.grantReadWrite(identity)
    }

    new CfnOutput(this, 'BucketName', {
      value: bucket.bucketName,
    })

    new CfnOutput(this, 'BucketUrl', {
      value: `https://${bucket.bucketName}.s3.${this.region}.amazonaws.com`,
    })
  }
}
