# Software Testing

## CI

[![Playwright Tests](https://github.com/fomap/SoftwareTesting/actions/workflows/playwright.yml/badge.svg?branch=main)](https://github.com/fomap/SoftwareTesting/actions/workflows/playwright.yml)

**CI system:** GitHub Actions. It is built into GitHub, where the repository already lives, needs no server of its own, is free for public repositories, and supports matrices for parallel jobs.

**Pipeline** (`.github/workflows/playwright.yml`):
- runs on every push and pull request (plus manual runs via `workflow_dispatch`);
- starts a local copy of the Toolshop app (sprint 5) in Docker, because the live site blocks GitHub runners;
- restores NuGet packages with caching (`setup-dotnet` + `packages.lock.json`), builds, and installs Chromium with system dependencies;
- splits the full test suite across 4 parallel jobs. Each job lists all tests, takes every 4th one, and runs them with `--filter`. The filter only does the split; every test runs in exactly one job;
- the `merge-reports` job collects all shards' results into one `test-report` artefact;
- artefacts: `test-results-1…4` (JUnit, HTML, TRX, traces) and `test-report`; kept for 14 days.

**CI settings** (`ci.runsettings`, used only in CI): headless Chromium, 2 parallel test workers, no slow-motion. JUnit, HTML and TRX loggers.

### Runtime

| Configuration | Total run time | Test step (slowest job) | Run |
| --- | --- | --- | --- |
| 1 job, no sharding | 5 min 6 s | 0 min 12 s | [run](https://github.com/fomap/SoftwareTesting/actions/runs/45074331259) |
| 4 shards + merge | 5 min 5 s | 0 min 7 s | [run](https://github.com/fomap/SoftwareTesting/actions/runs/45071821962) |

Conclusion: Shardint did cut the test steps from 12 seconds down to 7, howerver the total run time barery changed (5m 6s vs 5min 5s) due to each job having to spend ~5 min on the same setup whereas test itself taking 7 seconds. Since 4 jobs run in parallel, the larger test suite would significantly benefit from this setup. 


