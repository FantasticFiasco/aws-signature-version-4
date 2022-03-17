interface HttpResponse {
  statusCode: number
  headers: { [name: string]: string }
  body: string
}

const BODY = JSON.stringify({
  firstName: 'John',
  surname: 'Doe',
  age: 42,
})

export const handler = async (): Promise<HttpResponse> => {
  return {
    statusCode: 200,
    headers: {
      'content-type': 'application/json',
    },
    body: BODY,
  }
}
