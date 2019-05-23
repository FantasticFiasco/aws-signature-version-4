# Test

## Integration tests

Integration tests targeting AWS can be found in the `Integration` directory. All integration tests are marked with the xUnit trait `Integration` and are excluded from the ReSharper test runner.

These tests depend on secrets that during CI is set as environment variables. For local development one can create a file in this directory called `local-integration-test-variables.txt`, with the following structure.

```
SOME_VARIABLE_1=some-value-1
SOME_VARIABLE_2=some-value-2
...
```

This file is ignored by Git and will not be committed.

## Test suite

The [AWS Test Suite](https://docs.aws.amazon.com/general/latest/gr/signature-v4-test-suite.html) can be found in the `TestSuite` directory. It contains various test suite scenarios as well as tests verifying that we depend on the latest version of the suite.

## Unit tests

Unit tests can be found in the `Unit` directory. These tests does not require any prerequisite and is included by the ReSharper test runner.
