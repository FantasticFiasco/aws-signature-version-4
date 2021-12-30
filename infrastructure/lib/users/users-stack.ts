import { CfnOutput, Stack, StackProps } from 'aws-cdk-lib'
import { CfnAccessKey, IRole, IUser, PolicyStatement, Role, User } from 'aws-cdk-lib/aws-iam'
import { Construct } from 'constructs'

export class UsersStack extends Stack {
  readonly userWithPermissions: IUser
  readonly userWithoutPermissions: IUser
  readonly roleWithPermissions: IRole

  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props)

    this.userWithPermissions = this.createUserWithPermissions()
    this.userWithoutPermissions = this.createUserWithoutPermissions()
    this.roleWithPermissions = this.createRoleWithPermissions()
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
      })
    )

    // Create outputs
    new CfnOutput(this, 'ApiGatewayRoleArn', {
      value: role.roleArn,
    })

    return role
  }
}
