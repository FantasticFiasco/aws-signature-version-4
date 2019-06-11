#!/usr/bin/env node
import 'source-map-support/register';

import { App } from '@aws-cdk/cdk';
import { ApiGatewayStack, PrincipalsStack, S3Stack } from '../lib';

const app = new App();

const usersStack = new PrincipalsStack(app, 'PrincipalsStack', {
    stackName: 'SigV4-Principals',
});

new ApiGatewayStack(app, 'ApiGatewayStack', {
    stackName: 'SigV4-ApiGateway',
});

new S3Stack(app, 'S3Stack', {
    readAccess: [
        usersStack.trustedRole,
        usersStack.trustedUser,
    ],
    stackName: 'SigV4-S3',
});
