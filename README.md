Overview
This project implements the Character Copy Kata in C#. It demonstrates reading characters from a source and copying them to a destination one character at a time. The solution includes batch processing and unit tests using NSubstitute for mocking dependencies.

Features
Character-by-Character Copy: Reads characters from a source and writes them to a destination until a newline (\n) is encountered.
Batch Processing: Reads and writes multiple characters at a time in batches, stopping at the first newline.
Unit Testing: Uses xUnit for unit testing and NSubstitute for mocking interfaces (ISource and IDestination).
Class Library: The solution has been structured as a class library, without a Main method, to be reusable and testable.


Clone the repository
Restore dependencies
Build the solution
Run the tests
