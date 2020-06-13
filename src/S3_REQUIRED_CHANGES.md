# S3 required changes

- Step 1 - Canonical request
  - Normalize URI paths according to RFC 3986. Remove redundant and relative path components. Each path segment must be URI-encoded twice (except for Amazon S3 which only gets URI-encoded once).
- Step 3 - Calculate signature
  - The signature calculations vary depending on the method you choose to transfer the request payload. See https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-auth-using-authorization-header.html for more information.
    - 
- Step 4 - Add signature
  - You can make requests and pass all request values in the query string, including signing information. This is sometimes referred to as a presigned URL, because it produces a single URL with everything required in order to make a successful call to AWS. It's commonly used in Amazon S3.
