# Accounting app

Concept app for doing accounting.

Built in C# with Blazor Web Assembly and ASP.NET Core.

## Goal

To build an accounting app with user experience in mind.

Thanks to my friend for this idea.

## Features
* View accounts (BAS plans) and balances
* View and create verifications
* View ledger entries

You can also:
* Register sale in one form

### Lacking
In order for this to be a functional program:
* A way to attach documents to verifications

Nice to have:
* A better template system
* More specialized views - for other business events

## Screenshots

### Accounts

List BAS accounts and their balances.

![Alt text](/Screenshots/Accounts.png "Accounts")

### Ledger

All the entries representing the financial events.

![Alt text](/Screenshots/Ledger.png "Ledger")

### Verifications

The program is verification-centric. You create a verification to which the entries are attached.

![Alt text](/Screenshots/Verifications.png "Verifications")

At this time, there is no way to attach documents.

### Register sales

Register a standardized sale that will be saved without worrying about what accounts should be used:

![Alt text](/Screenshots/RegisterSale.png "Register sale")

The resulting entries:

![Alt text](/Screenshots/SalesEntries.png "Sales entries")

## Project

It is built with C# and Blazor for Web Assembly, and it uses the MudBlazor component library for UI.

The actual logic is in the backend service, and data is stored in a Sqlite database.

The parts can be 

* Frontend Web App
* Backend with Web API
* Sqlite database 

## Development environment

You need to have .NET 6 SDK installed to build this project.

No other dependencies required. Not even Node.

In the terminal, while in Server directory, run the following command:

```
dotnet run
```