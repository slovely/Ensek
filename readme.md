# Ensek Test Project

Added Aspire to the app to make running it straightforward.  Should be able
to run the `Ensek.AppHost` project and have the app and DB spin up automatically (requires docker).

The parser has the start of some unit tests in `Ensek.Api.Tests` project.

Further questions/discussions
 
- No Authentication yet.
- Depending on size of files uploaded we should move the parsing/etc to a asynchronous background process.
- Create a client UI, other than the simple Razor Pages.
- Again, depending on file sizes, we may need to optimise database access in the upload controller method.
- Possibly introduce a repository interface to make unit-testing the controller logic easier.
- Add integration tests.
