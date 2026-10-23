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
 
 <PM_2>
 whats the difference between dotnet test --filter "FullyQualifiedName~SIS2.tests"
and running RegisterCustomerTests

how can i see what the test is doing and verify it (maybe add slowmo to testrun settings?)
 </PM_2>

 <PM_3>
 now you have to make the same job but for the following test cases

2) Log in  = Valid credentials reach the account page; invalid ones show the error and do not.                      
3) Buy a product = Add to cart -> cart shows the right item and quantity -> checkout completes.   
4) Compare two products  =  Open two products, capture name and price, assert the comparison you state.                  
5) Reset a password  = The forgot-password flow gives the expected confirmation for a known email, and an error for an invalid one. 


Ask clarifying questions if needed.
 </PM_3>



 <PM_4>
 "AddToCart_CartShowsItemAndQuantity_CheckoutCompletes
   Source: BuyProductTests.cs line 34
   Duration: 29,9 sec

  Message: 
Microsoft.Playwright.PlaywrightException : Locator expected to have text matching regex 'Thanks for your order! Your invoice number is INV-\d+'
But was: 'null' 
Call log:
LocatorAssertions.ToHaveTextAsync with timeout 15000ms
waiting for selector "text=/Thanks for your order!/"

  Stack Trace: 
AssertionsBase.ExpectImplAsync(String expression, FrameExpectOptions expectOptions, Object expected, String message)
AssertionsBase.ExpectImplAsync(String expression, ExpectedTextValue[] expectedText, Object expected, String message, FrameExpectOptions options)
AssertionsBase.ExpectImplAsync(String expression, ExpectedTextValue textValue, Object expected, String message, FrameExpectOptions options)
BuyProductTests.AddToCart_CartShowsItemAndQuantity_CheckoutCompletes() line 82
GenericAdapter`1.BlockUntilCompleted()
NoMessagePumpStrategy.WaitForCompletion(AwaitAdapter awaiter)
AsyncToSyncAdapter.Await(Func`1 invoke)
TestMethodCommand.RunTestMethod(TestExecutionContext context)
TestMethodCommand.Execute(TestExecutionContext context)
<>c__DisplayClass1_0.<Execute>b__0()
DelegatingTestCommand.RunTestMethodInThreadAbortSafeZone(TestExecutionContext context, Action action)"

 </PM_4>


 <PM_5>
 the AddToCart_CartShowsItemAndQuantity_CheckoutCompletes fails because product put 
 into the Products is Out of stock, how to change the test so it takes it into account? (ask what do they do in prod)
 </PM_5>