interface Response {
  statusCode: number
}

export const handler = async (): Promise<Response> => {
  return {
    statusCode: 200,
  }
}
