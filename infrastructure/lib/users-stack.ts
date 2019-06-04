import { CfnAccessKey, IRole, IUser, PolicyStatement, Role, User } from '@aws-cdk/aws-iam';
import { CfnOutput, Construct, Stack, StackProps } from '@aws-cdk/cdk';

export class UsersStack extends Stack {

  public readonly trustedUser: IUser;
  public readonly untrustedUser: IUser;
  public readonly trustedRole: IRole;

  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props);

    this.trustedUser = this.createTrustedUser();
    this.untrustedUser = this.createUntrustedUser();
    this.trustedRole = this.createTrustedRole();

    const trustedUserAccessKey = new CfnAccessKey(this, 'trustedUserAccessKey', {
      userName: this.trustedUser.userName,
    });
    const untrustedUserAccessKey = new CfnAccessKey(this, 'untrustedUserAccessKey', {
      userName: this.untrustedUser.userName,
    });

    new CfnOutput(this, 'TrustedUserAccessKeyId', {
      value: trustedUserAccessKey.accessKeyId,
    });
    new CfnOutput(this, 'TrustedUserSecretAccessKey', {
      value: trustedUserAccessKey.accessKeySecretAccessKey,
    });
    new CfnOutput(this, 'UntrustedUserAccessKeyId', {
      value: untrustedUserAccessKey.accessKeyId,
    });
    new CfnOutput(this, 'UntrustedUserSecretAccessKey', {
      value: untrustedUserAccessKey.accessKeySecretAccessKey,
    });
  }

  private createTrustedUser(): IUser {
    const user = new User(this, 'TrustedUser', {
      userName: 'trusted-user',
    });

    user.attachManagedPolicy('arn:aws:iam::aws:policy/AmazonAPIGatewayInvokeFullAccess');

    return user;
  }

  private createUntrustedUser(): IUser {
    return new User(this, 'UntrustedUser', {
      userName: 'untrusted-user',
    });
  }

  private createTrustedRole(): IRole {
    const role = new Role(this, 'ApiGatewayRole', {
      assumedBy: this.untrustedUser,
      roleName: 'ApiGatewayInvoke',
    });

    role.addToPolicy(new PolicyStatement()
      .addActions('execute-api:Invoke', 'execute-api:ManageConnections')
      .addResource('arn:aws:execute-api:*:*:*'));

    return role;
  }
}
