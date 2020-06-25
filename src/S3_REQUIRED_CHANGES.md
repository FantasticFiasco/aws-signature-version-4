# S3 required changes

- Step 1 - Canonical request
  - Normalize URI paths according to RFC 3986. Remove redundant and relative path components. Each path segment must be URI-encoded twice (except for Amazon S3 which only gets URI-encoded once).
- Step 3 - Calculate signature
  - The signature calculations vary depending on the method you choose to transfer the request payload. See https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-auth-using-authorization-header.html for more information.
    - 
- Step 4 - Add signature
  - You can make requests and pass all request values in the query string, including signing information. This is sometimes referred to as a presigned URL, because it produces a single URL with everything required in order to make a successful call to AWS. It's commonly used in Amazon S3.



# Limitations

Object keys with special characters are not supported, they are the following:

- Ampersand ("&")
- Dollar ("$")
- ASCII character ranges 00–1F hex (0–31 decimal) and 7F (127 decimal)
- 'At' symbol ("@")
- Equals ("=")
- Semicolon (";")
- Colon (":")
- Plus ("+")
- Space – Significant sequences of spaces might be lost in some uses (especially multiple spaces)
- Comma (",")
- Question mark ("?")
- Backslash ("\")
- Left curly brace ("{")
- Non-printable ASCII characters (128–255 decimal characters)
- Caret ("^")
- Right curly brace ("}")
- Percent character ("%")
- Grave accent / back tick ("`")
- Right square bracket ("]")
- Quotation marks
- 'Greater Than' symbol (">")
- Left square bracket ("[")
- Tilde ("~")
- 'Less Than' symbol ("<")
- 'Pound' character ("#")
- Vertical bar / pipe ("|")

Implementation uses single chunk signature calculation, which is more taxing to the memory than multiple chunks signature calculation. For more information read https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-auth-using-authorization-header.html

Supports GET, PUT and DELETE
Does not support Browser-Based Uploads Using POST. For more information read https://docs.aws.amazon.com/AmazonS3/latest/API/sigv4-UsingHTTPPOST.html
