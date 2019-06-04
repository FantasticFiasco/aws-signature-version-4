#!/usr/bin/env node
import 'source-map-support/register';

import { App } from '@aws-cdk/cdk';
import { ApiGatewayStack, S3Stack, UsersStack } from '../lib';

const app = new App();

const usersStack = new UsersStack(app, 'UsersStack', {
    stackName: 'SigV4-Users',
});

new ApiGatewayStack(app, 'ApiGatewayStack', {
    stackName: 'SigV4-ApiGateway',
});

new S3Stack(app, 'S3Stack', {
    roleWithReadAccess: usersStack.trustedRole,
    stackName: 'SigV4-S3',
    userWithReadAccess: usersStack.trustedUser,
});
