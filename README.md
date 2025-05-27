# CronParser

CronParser is a command-line application written in C# that parses a standard cron expression and expands each field into its full list of scheduled values. The output is formatted as a readable table, similar to how a user might visualize a cron job's schedule.

## Features

- Parses standard 5-field cron expressions: minute, hour, day of month, month, day of week
- Supports:
  - Wildcards (`*`)
  - Step values (`*/N`)
  - Ranges (`M-N`)
  - Lists (`M,N,X`)
  - Combined expressions (e.g., `1-5,10,*/15`)
- Produces clean, aligned table output
- Designed to be modular and testable
- Includes a full suite of automated tests

## Requirements

- .NET 8.0 SDK or later

## Usage

Run the project using the .NET CLI:

`dotnet run --project CronParser "*/15 0 1,15 * 1-5 /usr/bin/find"`

Expected out from this is:

```text
minute 0 15 30 45
hour 0
day of month 1 15
month 1 2 3 4 5 6 7 8 9 10 11 12
day of week 1 2 3 4 5
command /usr/bin/find
```

## Running Tests

The project includes unit tests using xUnit and Shouldly.

To run all tests:

`dotnet test`

## Limitations

- Does not support special cron shortcuts like `@yearly`, `@reboot`, etc.
- Does not evaluate whether a job should run at a specific time

## Example

`dotnet run -- "*/10 9-17 * * 1-5 echo 'Hello World'"`

This represents a command that runs every 10 minutes during working hours on weekdays.
