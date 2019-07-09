#!/usr/bin/env node
import 'source-map-support/register';

import { App } from '@aws-cdk/core';
import { ApiGatewayStack, UsersStack } from '../lib';

const app = new App();

new UsersStack(app, 'UsersStack', {
    stackName: 'SigV4-Users',
});

new ApiGatewayStack(app, 'ApiGatewayStack', {
    stackName: 'SigV4-ApiGateway',
});

// new S3Stack(app, 'S3Stack', {
//     readAccess: [
//         usersStack.trustedRole,
//         usersStack.trustedUser,
//     ],
//     stackName: 'SigV4-S3',
// });
