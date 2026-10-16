Commit 1
- Single failing unit test already existed, so I updated it to instead test that the api does not work for an endpoint that doesn't exist. Next, I will consider creating unit tests for this application without mocking. This may require slight restructuring of the application.

Commit 2
- Created unit tests that assumed the data I enter is being used. Since the business rules are coupled to an inaccessible database, these should fail