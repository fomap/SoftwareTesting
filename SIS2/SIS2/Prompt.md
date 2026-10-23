<PM_1>
Your task is to create automated test using playwright and dotnet. The test case you need to test is following:
Test case; Register a new customer
What it must prove:  A unique new account is created and the user ends up signed in or on the login page.   

Here are requirements for the test:
- Every test has an assertion that could fail, a test with no meaningful assertion is useless. 
- No waitForTimeout anywhere. Not one. 
-No CSS classes and no positional XPath in locators.
-Tests must pass when run in any order and in parallel. 


For registration you will need to generate test data, here are the requirements:
- Registration needs an email nobody has used. Generate a fresh one every run — a timestamp suffix, or faker.
- Never reuse a hard-coded new account between runs. The second run fails with «email already exists» and you will blame Playwright.
- For tests that need an existing account, use the seeded ones: customer@practicesoftwaretesting.com / welcome01, and customer2@practicesoftwaretesting.com / welcome01
- Keep data out of the spec. A factory function in data/ returns a fresh user object; the test just asks for one.
- This is a shared public application. Other people are using it at the same time as you — never write a test that assumes it is the only user.

Ask clarifying questions if needed.
</PM_1>