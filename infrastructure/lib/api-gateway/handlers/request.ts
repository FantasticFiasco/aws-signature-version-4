import { APIGatewayEvent } from 'aws-lambda'

interface HttpResponse {
  statusCode: number
  headers: { [name: string]: string }
  body: string
}

interface ReceivedRequest {
  method: string
  path: string
  headers: { [name: string]: string | undefined } | null
  queryParameters: { [name: string]: string | undefined } | null
  body: string | null
}

export const handler = async ({ httpMethod, path, headers, queryStringParameters, body }: APIGatewayEvent): Promise<HttpResponse> => {
  const receivedRequest: ReceivedRequest = {
    method: httpMethod,
    path,
    headers,
    queryParameters: queryStringParameters,
    body,
  }

  return {
    statusCode: 200,
    headers: {
      'content-type': 'application/json',
    },
    body: JSON.stringify(receivedRequest),
  }
}
