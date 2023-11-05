import { CfnOutput, Stack, StackProps } from 'aws-cdk-lib'
import { CfnAccessKey, Effect, IRole, IUser, ManagedPolicy, PolicyStatement, Role, User } from 'aws-cdk-lib/aws-iam'
import { Construct } from 'constructs'

export class UsersStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props)

    // Policy that grants full access to test buckets
    const testBucketPolicy = this.createS3FullAccessPolicy()

    // IAM user with permissions to provision buckets for individual test runs
    this.createUserWithProvisioningPermissions()

    // IAM user with permissions to access the API Gateway
    this.createUserWithPermissions(testBucketPolicy)

    // IAM user without permissions to access the API Gateway
    const userWithoutPermissions = this.createUserWithoutPermissions()

    // IAM role with permissions to access the API Gateway
    this.createRoleWithPermissions(userWithoutPermissions, testBucketPolicy)
  }

  private createS3FullAccessPolicy(): ManagedPolicy {
    return new ManagedPolicy(this, 'TestBucketPolicy', {
      managedPolicyName: 'sigv4-TestBucketPolicy',
      statements: [
        new PolicyStatement({
          effect: Effect.ALLOW,
          actions: ['s3:*'],
          // resources: ['arn:aws:s3:::sigv4-test-*']
          resources: ['*'],
        }),
      ],
    })
  }

  private createUserWithProvisioningPermissions(): IUser {
    // Create user
    const user = new User(this, 'UserWithProvisioningPermissions', {
      userName: 'sigv4-UserWithProvisioningPermissions',
    })

    user.addManagedPolicy({
      managedPolicyArn: 'arn:aws:iam::aws:policy/AmazonS3FullAccess',
    })

    // Create access key
    const accessKey = new CfnAccessKey(this, 'UserWithProvisioningPermissionsAccessKey', {
      userName: user.userName,
    })

    // Create outputs
    new CfnOutput(this, 'UserWithProvisioningPermissionsAccessKeyId', {
      value: accessKey.ref,
    })

    new CfnOutput(this, 'UserWithProvisioningPermissionsSecretAccessKey', {
      value: accessKey.attrSecretAccessKey,
    })

    return user
  }

  private createUserWithPermissions(testBucketPolicy: ManagedPolicy): IUser {
    // Create user
    const user = new User(this, 'UserWithPermissions', {
      userName: 'sigv4-UserWithPermissions',
    })

    // API Gateway permissions
    user.addManagedPolicy({
      managedPolicyArn: 'arn:aws:iam::aws:policy/AmazonAPIGatewayInvokeFullAccess',
    })

    // S3 permissions
    user.addManagedPolicy(testBucketPolicy)

    // Create access key
    const accessKey = new CfnAccessKey(this, 'UserWithPermissionsAccessKey', {
      userName: user.userName,
    })

    // Create outputs
    new CfnOutput(this, 'UserWithPermissionsAccessKeyId', {
      value: accessKey.ref,
    })

    new CfnOutput(this, 'UserWithPermissionsSecretAccessKey', {
      value: accessKey.attrSecretAccessKey,
    })

    return user
  }

  private createUserWithoutPermissions(): IUser {
    // Create user
    const user = new User(this, 'UserWithoutPermissions', {
      userName: 'sigv4-UserWithoutPermissions',
    })

    // Create access key
    const accessKey = new CfnAccessKey(this, 'UserWithoutPermissionsAccessKey', {
      userName: user.userName,
    })

    // Create outputs
    new CfnOutput(this, 'UserWithoutPermissionsAccessKeyId', {
      value: accessKey.ref,
    })

    new CfnOutput(this, 'UserWithoutPermissionsSecretAccessKey', {
      value: accessKey.attrSecretAccessKey,
    })

    return user
  }

  private createRoleWithPermissions(user: IUser, testBucketPolicy: ManagedPolicy): IRole {
    const role = new Role(this, 'RoleWithPermissions', {
      assumedBy: user,
      roleName: 'sigv4-RoleWithPermissions',
    })

    // API Gateway permissions
    role.addToPolicy(
      new PolicyStatement({
        actions: ['execute-api:Invoke', 'execute-api:ManageConnections'],
        resources: ['arn:aws:execute-api:*:*:*'],
      }),
    )

    // S3 permissions
    role.addManagedPolicy(testBucketPolicy)

    // Create outputs
    new CfnOutput(this, 'RoleWithPermissionsArn', {
      value: role.roleArn,
    })

    return role
  }
}
