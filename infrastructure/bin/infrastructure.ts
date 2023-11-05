#!/usr/bin/env node

import { App } from 'aws-cdk-lib'
import 'source-map-support/register'
import { ApiGatewayStack, UsersStack } from '../lib'

const app = new App()

new UsersStack(app, 'UsersStack', {
  stackName: 'SigV4-Users',
})

new ApiGatewayStack(app, 'ApiGatewayStack', {
  stackName: 'SigV4-ApiGateway',
})
