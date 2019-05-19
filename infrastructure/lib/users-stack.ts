import { PolicyStatement, Role, User } from '@aws-cdk/aws-iam';
import { Construct, Stack, StackProps } from '@aws-cdk/cdk';

export class UsersStack extends Stack {
  constructor(scope: Construct, id: string, props?: StackProps) {
    super(scope, id, props);

    // User with API Gateway permissions
    const userWithApiGatewayPermissions = new User(this, 'UserWithApiGatewayPermissions', {
      userName: 'user-with-api-gateway-permissions',
    });

    userWithApiGatewayPermissions.attachManagedPolicy('arn:aws:iam::aws:policy/AmazonAPIGatewayInvokeFullAccess');

    // User without API Gateway permissions, but with a role to assume them
    const userWithoutApiGatewayPermissions = new User(this, 'UserWithoutApiGatewayPermissions', {
      userName: 'user-without-api-gateway-permissions',
    });

    const apiGatewayRole = new Role(this, 'ApiGatewayRole', {
      assumedBy: userWithoutApiGatewayPermissions,
      roleName: 'ApiGatewayInvoke',
    });

    apiGatewayRole.addToPolicy(new PolicyStatement()
      .addActions('execute-api:Invoke', 'execute-api:ManageConnections')
      .addResource('arn:aws:execute-api:*:*:*'));
  }
}
