#!/usr/bin/env node
import 'source-map-support/register';

import { App } from '@aws-cdk/cdk';
import { ApiGatewayStack, UsersStack } from '../lib';

const app = new App();

new UsersStack(app, 'UsersStack', {
    stackName: 'SigV4-Users',
 });
new ApiGatewayStack(app, 'ApiGatewayStack', {
    stackName: 'SigV4-ApiGateway',
});
