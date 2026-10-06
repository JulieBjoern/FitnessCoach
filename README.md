# FitnessCoach

A Blazor web app that generates a personal weekly training plan with AI, based on your training profile and any injuries you have. It runs both with real AI (OpenAI) and in a free "mock" mode.

> **Work in progress.** This is a portfolio project, built in small vertical slices and extended step by step.

## Features

- [x] .NET Aspire orchestration with a dashboard for logs, traces and health
- [x] MudBlazor UI shell with navigation
- [x] Structured logging with Serilog (console + file + Aspire dashboard)
- [ ] Training profile: goal, experience level, biological sex, training days per week and session length
- [ ] Injury registry with full CRUD and "mark as healed" (keeps history)
- [ ] Dashboard with profile summary, active injuries and latest plan
- [ ] AI-generated weekly training plans with plan history
- [ ] Switch between OpenAI and a free mock generator via configuration

## Tech stack

| Area | Technology |
|---|---|
| Web | ASP.NET Core Blazor (.NET 10, Interactive Server) |
| UI components | MudBlazor |
| Orchestration | .NET Aspire (AppHost + ServiceDefaults) |
| Data | Entity Framework Core + SQL Server (Docker container managed by Aspire) |
| AI | OpenAI Chat Completions API |
| Logging | Serilog + OpenTelemetry |
| Validation | Data Annotations |
| Testing | xUnit |

## Architecture

```
Blazor pages  →  services (DI)  →  EF Core  →  SQL Server (container)
                      │
                      └─→  IWorkoutPlanGenerator
                              ├─ OpenAiWorkoutPlanGenerator  (real AI)
                              └─ MockWorkoutPlanGenerator    (free, deterministic)
```

- **No controllers.** The Blazor pages call services, which are registered in dependency injection.
- **AI behind an interface.** A config setting decides whether plans come from OpenAI or the mock. That makes it possible to develop, test and host a public demo without spending API tokens.
- **Single-user by design.** There is no login and only one profile, so the scope stays focused on the database and AI integration.

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Aspire CLI](https://aspire.dev)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/). Required once the database is added, because SQL Server runs in a container.

### Run the app

```bash
aspire run
```

The console shows a link to the Aspire dashboard. Open the app from there by clicking the `fitnesscoach-web` endpoint. Stop the app with `Ctrl+C`.

## Configuration

| Setting | Where | Description |
|---|---|---|
| `Serilog:MinimumLevel` | `appsettings.json` | Log levels |
| `AiProvider` *(planned)* | `appsettings.{Environment}.json` | `OpenAI` or `Mock` |
| `openai-apikey` *(planned)* | AppHost user secrets | OpenAI API key, stored as an Aspire secret parameter |

Secrets are never stored in `appsettings.json` or committed to git. Aspire provides the database connection string automatically.

## Project structure

```
aspire/
  FitnessCoach.AppHost/          Aspire AppHost: orchestrates the app and its resources
  FitnessCoach.ServiceDefaults/  Shared defaults: OpenTelemetry, health checks, resilience
src/
  FitnessCoach.Web/              Blazor web app
```

## Logging

Logs are written to three places:

- the console
- a daily rolling file in `src/FitnessCoach.Web/logs/`
- the **Structured logs** view in the Aspire dashboard

## Roadmap

1. ✅ Foundation: Aspire, Blazor, MudBlazor, Serilog
2. Database and training profile
3. Injury registry
4. Dashboard
5. Weekly plans with a mock AI and plan history
6. OpenAI integration, error handling and loading state

Later ideas include caching AI responses, exporting plans (PDF/calendar), a public demo on Azure, and CI/CD with GitHub Actions.
