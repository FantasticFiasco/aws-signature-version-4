export const handler = async (event: any, context: any) => {
    return {
        body: JSON.stringify(event),
        statusCode: 200,
    };
};
