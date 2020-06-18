#!/usr/bin/env node
import { App } from '@aws-cdk/core';
import 'source-map-support/register';
import { ApiGatewayStack, S3Stack, UsersStack } from '../lib';

const app = new App();

const usersStack = new UsersStack(app, 'UsersStack', {
    stackName: 'SigV4-Users',
});

new ApiGatewayStack(app, 'ApiGatewayStack', {
    stackName: 'SigV4-ApiGateway',
});

new S3Stack(app, 'S3Stack', {
    readWriteAccess: [usersStack.userWithPermissions, usersStack.roleWithPermissions],
    stackName: 'SigV4-S3',
});
