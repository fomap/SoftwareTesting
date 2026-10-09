## A one-page test plan section: scope in and out, approach, entry and exit criteria, top three product risks.

This testing plan is for the money transferring feature for the SigmaBankApp mobile application version 0.1. The objective of this plan is to ensure that the feature meets the demanded requirements and is free of defects. 

**Scope** includes testing operations related to the money transferring feature. Anything not related to transferring, bank account balance, daily limits and code verification is excluded. 

**Features** to be tested:
- User transferring amounts defined in equivalence partitions, boundary values. 
- SMS code verification
- Limit equal to 1 000 000 KZT across all transfers
- 120 seconds valid time window
- 3 attempts functionality
- Limit and valid transfer amount violation error window
- Test strategy - analytical strategy, requirement based testing. 

**Automated testing:** 
- entering valid and invalid transfer values
- entering correct SMS code entry 
- entering SMS code entry at exactly 120 seconds

**Manual testing:**
- entering incorrect SMS code entry
- transferring above limit 
- transferring invalid transfer amount


**Test environment:** SigmaBankApp mobile application of 0.1 build version. The provided accounts are: user066 user067, user068. 

**Test schedule and milestones:**

|   Phase   |       Milestones        | Entry and exit criterias                                                                                                                                                                    |
| :-------: | :---------------------: | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Planning  | 02.11.2026 - 13.11.2026 | Entry criteria: software requirement are defined, test strategy and test plan are finalized<br>Exit criteria: test plan is approved, testing team is assembled.                             |
| Execution | 16.11.2026 - 27.11.2026 | Entry criteria: test environment is set up, test accounts are registered and have set test account balance. <br>Exit criteria: all test cases are concluded, log saved and defects tracked. |
| Reporting | 30.11.2026 - 04.12.2026 | Entry criteria: execution phases is completed, defects are resolved or deferred for next sprint. <br>Exit criteria: test summary and bug reports are finalized.                             |

**Top 3 risks** 
* Broken core feature of the SigmaBankApp
* Increased software development cost 
* Delayed launch of the SigmaBankApp
## Twelve test cases from your week 3 design work, in full format, each traced to a requirement.

| Test ID |                     Traces to                     |                       Preconditions                       |                                              Test data / steps                                             |                                                                                Expected result                                                                               |
|:-------:|:-------------------------------------------------:|:---------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------:|:----------------------------------------------------------------------------------------------------------------------------------------------------------------------------:|
| TC-01   | REQ-AMT; DEC-R2                                   | User authenticated; daily total = 0 KZT; valid recipient. | Transfer 100 KZT whole tenge.                                                                              | Transfer executes without SMS. Daily total becomes 100 KZT.                                                                                                                  |
| TC-02   | REQ-AMT; REQ-SMS; DEC-R1; ST-4                    | Daily total = 0 KZT; SMS service available.               | Transfer 500,000 KZT. Receive SMS and enter correct 6-digit code within 120 s.                             | SMS code requested. After correct code, transfer executes. Daily total becomes 500,000 KZT.                                                                                  |
| TC-03   | REQ-AMT; DEC-R6                                   | Daily total = 0 KZT.                                      | Transfer 99 KZT.                                                                                           | Rejected: amount not in limit. No SMS. Daily total unchanged.                                                                                                                |
| TC-04   | REQ-AMT; DEC-R5                                   | Daily total = 0 KZT.                                      | Transfer 500,001 KZT.                                                                                      | Rejected: amount not in limit. No SMS. Daily total unchanged.                                                                                                                |
| TC-05   | REQ-AMT whole tenge only                          | Daily total = 0 KZT.                                      | Transfer 100.50 KZT.                                                                                       | Rejected: amount must be whole tenge / invalid amount. No transfer.                                                                                                          |
| TC-06   | REQ-DAILY; DEC-R1; ST-4                           | Daily total already = 600,000 KZT.                        | Transfer 400,000 KZT. Enter correct SMS code within 120 s.                                                 | SMS requested. Transfer executes because 600,000 + 400,000 = 1,000,000 KZT, exactly at limit. Daily total becomes 1,000,000 KZT.                                             |
| TC-07   | REQ-DAILY; DEC-R3                                 | Daily total already = 700,000 KZT.                        | Transfer 400,000 KZT.                                                                                      | Rejected: daily total limit exceeded. No SMS. No transfer. Daily total remains 700,000 KZT.                                                                                  |
| TC-08   | REQ-DAILY reset; DEC-R4 then R2                   | Before midnight Almaty, daily total = 950,000 KZT.        | At 23:59 Almaty, transfer 100,000 KZT → rejected. After 00:01 Almaty next day, transfer 100,000 KZT again. | First attempt rejected: daily limit exceeded. After midnight Almaty reset, second attempt executes without SMS. New daily total becomes 100,000 KZT.                         |
| TC-09   | REQ-PREC; DEC-R7                                  | Daily total already = 900,000 KZT.                        | Transfer 600,000 KZT. Amount is invalid and daily total would become 1,500,000 KZT.                        | Rejected with amount not in limit error, not daily limit error. No SMS.                                                                                                      |
| TC-10   | REQ-SMS threshold; DEC-R2                         | Daily total = 0 KZT.                                      | Transfer 100,000 KZT exactly.                                                                              | Transfer executes without SMS because amount is not above 100,000 KZT. Daily total becomes 100,000 KZT.                                                                      |
| TC-11   | REQ-SMS; REQ-CODE; ST-1 → ST-2 → ST-3 → Cancelled | Daily total = 0 KZT; valid recipient.                     | Transfer 150,000 KZT. When SMS code requested, enter incorrect 6-digit code three times.                   | Attempt #1 → Attempt #2 after 1st wrong code. Attempt #2 → Attempt #3 after 2nd wrong code. After 3rd wrong code, transfer is Cancelled. No transfer. Daily total unchanged. |
| TC-12   | REQ-CODE; ST-7/8/9 expiry                         | Daily total = 0 KZT; valid recipient.                     | Transfer 150,000 KZT. When SMS code requested, wait more than 120 seconds before entering code.            | SMS code expires. Transfer state becomes Expired. No transfer. Daily total unchanged.                                                                                        |
## A traceability matrix — including any requirement you cannot cover, and why.


|                            Traceable item                             | TC-01 | TC-02 | TC-03 | TC-04 | TC-05 | TC-06 | TC-07 | TC-08 | TC-09 | TC-10 | TC-11 | TC-12 | Notes                                                                                                                                                                           |
| :-------------------------------------------------------------------: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | :---: | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|          REQ-AMT — Amount 100–500,000 KZT, whole tenge only           |   X   |   X   |   X   |   X   |   X   |       |       |       |       |   X   |       |       |                                                                                                                                                                                 |
|     REQ-DAILY — Daily limit 1,000,000 KZT, reset midnight Almaty      |       |       |       |       |       |   X   |   X   |   X   |       |       |       |       |                                                                                                                                                                                 |
|         REQ-SMS — Above 100,000 KZT requires 6-digit SMS code         |       |   X   |       |       |       |   X   |       |       |       |   X   |   X   |   X   |                                                                                                                                                                                 |
|          REQ-CODE — Code valid 120s; 3 wrong attempts cancel          |       |       |       |       |       |       |       |       |       |       |   X   |   X   | Three wrong attempts covered. Expiry covered only from Attempt #1. Exact 120s boundary, correct code after 1–2 wrong attempts, and expiry from Attempt #2 / #3 are not covered. |
| REQ-PREC — If amount and daily limit both violated, show amount error |       |       |       |       |       |       |       |       |   X   |       |       |       | Covers both violated when amount > 100,000 KZT. Both violated when amount ≤ 100,000 KZT is not covered.                                                                         |
|                                                                       |       |       |       |       |       |       |       |       |       |       |       |       |                                                                                                                                                                                 |
|                       DEC-R1 — Ask for SMS code                       |       |   X   |       |       |       |   X   |       |       |       |       |       |       |                                                                                                                                                                                 |
|                       DEC-R2 — Execute transfer                       |   X   |       |       |       |       |       |       |   X   |       |   X   |       |       |                                                                                                                                                                                 |
|              DEC-R3 — Reject: daily total limit exceeded              |       |       |       |       |       |       |   X   |       |       |       |       |       |                                                                                                                                                                                 |
|              DEC-R4 — Reject: daily total limit exceeded              |       |       |       |       |       |       |       |   X   |       |       |       |       |                                                                                                                                                                                 |
|                 DEC-R5 — Reject: amount not in limit                  |       |       |       |   X   |       |       |       |       |       |       |       |       |                                                                                                                                                                                 |
|                 DEC-R6 — Reject: amount not in limit                  |       |       |   X   |       |       |       |       |       |       |       |       |       |                                                                                                                                                                                 |
|         DEC-R7 — Reject: amount not in limit (both violated)          |       |       |       |       |       |       |       |       |   X   |       |       |       |                                                                                                                                                                                 |
|         DEC-R8 — Reject: amount not in limit (both violated)          |       |       |       |       |       |       |       |       |       |       |       |       | Not covered, need to test when daily total is already 1 000 000 KZT and transfer 50 KZT. Expected: amount error, not daily-limit error.                                         |
|                                                                       |       |       |       |       |       |       |       |       |       |       |       |       |                                                                                                                                                                                 |
|              ST-1 — Attempt #1 + Incorrect → Attempt #2               |       |       |       |       |       |       |       |       |       |       |   X   |       |                                                                                                                                                                                 |
|              ST-2 — Attempt #2 + Incorrect → Attempt #3               |       |       |       |       |       |       |       |       |       |       |   X   |       |                                                                                                                                                                                 |
|               ST-3 — Attempt #3 + Incorrect → Cancelled               |       |       |       |       |       |       |       |       |       |       |   X   |       |                                                                                                                                                                                 |
|                ST-4 — Attempt #1 + Correct → Confirmed                |       |   X   |       |       |       |   X   |       |       |       |       |       |       |                                                                                                                                                                                 |
|                ST-5 — Attempt #2 + Correct → Confirmed                |       |       |       |       |       |       |       |       |       |       |       |       | Not covered, need to test one wrong code then correct code.                                                                                                                     |
|                ST-6 — Attempt #3 + Correct → Confirmed                |       |       |       |       |       |       |       |       |       |       |       |       | Not covered, need to test two wrong codes then correct code.                                                                                                                    |
|                   ST-7/8/9 — wait > 120s → Expired                    |       |       |       |       |       |       |       |       |       |       |       |   X   |                                                                                                                                                                                 |
## A release checklist for the SMS code flow, at most twelve items.

1. Confirm that SMS code is able to be received to any KZ operator 
2. Verify that correct SMS code entries works when triggered.
3. Verify that incorrect SMS code entry triggers another attempt or cancels transaction.
4. Deploy SMS code feature to production app. 

## Three defect reports, you can mock them or design from lack of requirements.
	



## AI appendix (Level 1): prompts used, raw output, what you changed and why.


Task 1: 
Prompt used: Here is a description for the bank application that has following limitation regarding money transfers:

Amount: 100 to 500,000 KZT per transfer, whole tenge only.
Daily limit: 1,000,000 KZT across all transfers, reset at midnight Almaty time.
Transfers above 100,000 KZT require a 6-digit SMS code.
The code is valid for 120 seconds. Three wrong attempts cancel the transfer.
If the amount and the daily limit are both violated, the amount error is shown.

Here is decision table:

|             |                                    | R1 | R2 | R3 | R4 | R5 | R6 | R7 | R8 |
|-------------|------------------------------------|----|----|----|----|----|----|----|----|
| Condition 1 | Amount is between 100 - 500 000    |  T |  T |  T |  T |  F |  F |  F |  F |
| Condition 2 | Daily total transfer <= 1 000 000  |  T |  T |  F |  F |  T |  T |  F |  F |
| Condition 3 | Transfer amount above 100 000      |  T |  F |  T |  F |  T |  F |  T |  F |
| Action 1    | Execute                            |    |  X |    |    |    |    |    |    |
| Action 2    | Reject: daily total limit exceeded |    |    |  X |  X |    |    |    |    |
| Action 3    | Reject: amount not in limit        |    |    |    |    |  X |  X |  X |  X |
| Action 4    | Ask for SMS code                   |  X |    |    |    |    |    |    |    |


as well as state table:

|                     |     Events     |                |             |
|---------------------|:--------------:|:--------------:|:-----------:|
|        State        | Incorrect code |  Correct code  | wait > 120s |
|    S1: Attempt #1   |   Attempt #2   | Transfer money |   Expired   |
|    S2: Attempt #2   |   Attempt #3   | Transfer money |   Expired   |
|    S3: Attempt #3   |    Cancelled   | Transfer money |   Expired   |
| S4: Cancel transfer |       N/A      |       N/A      |     N/A     |
|  S5: Transfer money |       N/A      |       N/A      |     N/A     |
|     S6: Expired     |       N/A      |       N/A      |     N/A     |

and state transition table

|  #  |                             Transition                              |                |             |
| :-: | :-----------------------------------------------------------------: | :------------: | :---------: |
|  1  | Start state: Attempt #1 Event: Incorrect code End state: Attempt #2 |  Correct code  | wait > 120s |
|  2  | Start state: Attempt #2 Event: Incorrect code End state: Attempt #3 | Transfer money |   Expired   |
|  3  | Start state: Attempt #3 Event: Incorrect code End state: Cancelled  | Transfer money |   Expired   |
|  4  |  Start state: Attempt #1 Event: Correct code End state: Confirmed   | Transfer money |   Expired   |
|  5  |  Start state: Attempt #2 Event: Correct code End state: Confirmed   |      N/A       |     N/A     |
|  6  |  Start state: Attempt #3 Event: Correct code End state: Confirmed   |      N/A       |     N/A     |
|  7  |    Start state: Attempt #1 Event: wait > 120s End state: Expired    |      N/A       |     N/A     |
|  8  |    Start state: Attempt #2 Event: wait > 120s End state: Expired    |                |             |
|  9  |    Start state: Attempt #3 Event: wait > 120s End state: Expired    |                |             |
your task is to create twelve test cases based on the information below in full format, each traced to a requirement. End of prompt


Task 2:
Prompt used: from this test cases and requirements specified above, create a traceability matrix — including any requirement you cannot cover, and why. End of prompt. Result is on task 2.



References: 
* https://tryqa.com/what-is-test-strategy-types-of-strategies-with-examples/ 
* https://www.geeksforgeeks.org/software-testing/test-plan-software-testing/ 
* https://www.browserstack.com/guide/questions-to-ask-before-software-release

