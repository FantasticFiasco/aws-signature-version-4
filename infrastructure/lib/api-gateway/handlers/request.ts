import { APIGatewayEvent } from 'aws-lambda'

interface HttpResponse {
  statusCode: number
  headers: { [name: string]: string }
  body: string
}

interface ReceivedRequest {
  method: string
  path: string
  queryStringParameters: { [name: string]: string[] | undefined; } | null
  headers: { [name: string]: string[] | undefined; } | null
  body: string | null
}

export const handler = async (event: APIGatewayEvent): Promise<HttpResponse> => {
  const receivedRequest: ReceivedRequest = {
    method: event.httpMethod,
    path: event.path,
    queryStringParameters: event.multiValueQueryStringParameters,
    headers: event.multiValueHeaders,
    body: event.body,
  }

  return {
    statusCode: 200,
    headers: {
      'content-type': 'application/json',
    },
    body: JSON.stringify(receivedRequest),
  }
}
