import { CfnOutput, Stack, StackProps } from 'aws-cdk-lib'
import { CfnAccessKey, IRole, IUser, PolicyStatement, Role, User } from 'aws-cdk-lib/aws-iam'
import { Construct } from 'constructs'

export class UsersStack extends Stack {
  readonly userWithProvisioningPermissions: IUser
  readonly userWithPermissions: IUser
  readonly userWithoutPermissions: IUser
  readonly roleWithPermissions: IRole

  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props)

    // IAM user with permissions to provision buckets for individual test runs
    this.userWithProvisioningPermissions = this.createUserWithProvisioningPermissions()

    // IAM user with permissions to access the API Gateway
    this.userWithPermissions = this.createUserWithPermissions()

    // IAM user without permissions to access the API Gateway
    this.userWithoutPermissions = this.createUserWithoutPermissions()

    // IAM role with permissions to access the API Gateway
    this.roleWithPermissions = this.createRoleWithPermissions()
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

  private createUserWithPermissions(): IUser {
    // Create user
    const user = new User(this, 'UserWithPermissions', {
      userName: 'sigv4-UserWithPermissions',
    })

    user.addManagedPolicy({
      managedPolicyArn: 'arn:aws:iam::aws:policy/AmazonAPIGatewayInvokeFullAccess',
    })

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

  private createRoleWithPermissions(): IRole {
    const role = new Role(this, 'ApiGatewayRole', {
      assumedBy: this.userWithoutPermissions,
      roleName: 'sigv4-ApiGatewayInvoke',
    })

    role.addToPolicy(
      new PolicyStatement({
        actions: ['execute-api:Invoke', 'execute-api:ManageConnections'],
        resources: ['arn:aws:execute-api:*:*:*'],
      }),
    )

    // Create outputs
    new CfnOutput(this, 'ApiGatewayRoleArn', {
      value: role.roleArn,
    })

    return role
  }
}
