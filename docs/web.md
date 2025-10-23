# ArturRios.Common.Web

This document describes the `ArturRios.Common.Web` library: its purpose, features, folder layout, important types, and
typical usage patterns for wiring into an ASP.NET Core application.

Summary
-------
`ArturRios.Common.Web` contains reusable web helpers, middleware, HTTP gateway/client utilities, and security
primitives (JWT) that are used across web APIs and clients in this codebase.

Key features
------------

- Middleware for standardized error handling and request processing.
- JWT authentication middleware and token provider helpers.
- A small HTTP gateway and HTTP output wrapper to standardize calls to remote services.
- Base typed API client to make consuming downstream HTTP APIs easier and consistent.
- Startup wiring helpers and configuration keys for consistent app configuration.

Project structure (important folders and files)
-----------------------------------------------

- `Api/`
    - `Client/`
        - `BaseWebApiClient.cs` — base class to implement typed API clients.
        - `BaseWebApiClientRoute.cs` — helpers to construct routes for API clients.
    - `WebApiStartup.cs` — helper to configure and bootstrap Web API conventions (DI registration, middlewares).

- `AspNetCore/`
    - `ResponseResolver.cs` — maps internal outputs/exceptions to HTTP responses (status codes and payload shapes).

- `Http/`
    - `HttpGateway.cs` — a centralized HTTP helper to perform requests, handle serialization, and wrap results in
      `HttpOutput`.
    - `HttpOutput.cs` — standard output wrapper used by the gateway to carry status, data, and error details.
    - `HttpStatusCodes.cs` — constants used across the project for HTTP status handling.

- `Middleware/`
    - `ExceptionMiddleware.cs` — global exception handler that converts exceptions into standardized HTTP responses (
      often using `ResponseResolver`).
    - `WebApiMiddleware.cs` — base type for request-level middlewares.

- `Security/`
    - `Attributes/`
        - `AllowAnonymousAttribute.cs`, `AuthorizeAttribute.cs`, `RoleRequirementAttribute.cs` — attribute-based
          authorization helpers.
    - `Extensions/`
        - `AuthenticationExtensions.cs` — extension methods to register authentication services.
    - `Filters/`
        - `RoleRequirementFilter.cs` — filter enforcing role-based access used with the attributes.
    - `Interfaces/`
        - `IAuthenticationProvider.cs` — abstraction for a service that validates credentials or tokens.
    - `Middleware/`
        - `JwtMiddleware.cs` — middleware to extract/validate JWT tokens from requests and attach `AuthenticatedUser`
          information to the request context.
    - `Providers/`
        - `TokenProvider.cs` — helper to generate and validate JWT tokens.
    - `Records/`
        - `AuthenticatedUser.cs`, `Authentication.cs`, `Credentials.cs` — record types for auth state and credentials
          payloads.
    - `Validation/`
        - `CredentialsValidator.cs` — a validator for credentials payloads.

Important types and how they fit together
-----------------------------------------

- Http gateway and client
    - Use `HttpGateway` when you need a low-level helper to perform HTTP calls with consistent serialization, retries,
      and error mapping. It returns `HttpOutput<T>` which wraps the response data or error details.
    - Implement typed clients by deriving from `BaseWebApiClient<T>` (see `Api/Client/`). This provides helper methods
      and route-building utilities so each client focuses on API specifics.

- Middleware
    - Register `ExceptionMiddleware` early in the pipeline to ensure exceptions are captured and returned as friendly
      JSON responses with consistent status codes and logging.
    - `WebApiMiddleware` base type for any request-level middleware.

- Security
    - `JwtMiddleware` reads the `Authorization` header, validates the token (using `TokenProvider` and
      `IAuthenticationProvider`), and populates a request-scoped `AuthenticatedUser` record for controllers and other
      middleware to use.
    - Attributes like `AuthorizeAttribute` and `RoleRequirementAttribute` can be used on controllers/actions to gate
      access.

Configuration keys
------------------
`Configuration/AppSettingsKeys.cs` centralizes common configuration key names used by the web components (for example:
JWT issuer, signing key, token lifetime, downstream API base URLs). Use these keys when populating your
`appsettings.json` or environment variables to ensure consistent configuration names.

Typical usage (wiring into an ASP.NET Core app)
-----------------------------------------------
Below are the conceptual steps (the exact method names may differ; consult `WebApiStartup.cs` and
`AuthenticationExtensions.cs` for the precise signatures):

1. Add configuration values (appsettings.json / env vars):
    - Jwt settings (issuer, secret/key, audience, lifetime)
    - Downstream API base URLs
2. In `Program.cs` / `Startup.cs` register services:
    - Call the helper in `WebApiStartup` to register web services, or manually register `HttpGateway`, `TokenProvider`,
      and `IAuthenticationProvider` implementations.
    - Add authentication registration via methods in `Security/Extensions/AuthenticationExtensions.cs` if present.
3. Configure middleware pipeline:
    - Use `ExceptionMiddleware` to capture all exceptions.
    - Use `JwtMiddleware` for token extraction/validation.
4. Implement controllers or API clients that use `BaseWebApiClient` and `HttpGateway`.

Example (conceptual) middleware ordering in Program.cs:

- ExceptionMiddleware
- JwtMiddleware
- Routing / Authentication / Authorization
- Endpoints

Examples
--------

- Creating a typed API client:
    - Derive from `BaseWebApiClient` and pass the base URL and route builders. Use the base methods to issue GET/POST
      requests and get `HttpOutput<T>` responses.

- Generating a token:
    - Use `TokenProvider` to create JWT tokens for authenticated users; `JwtMiddleware` will validate and decode them on
      incoming requests.

Troubleshooting and tips
------------------------

- If responses are unexpectedly empty or error mapping looks wrong, inspect `ResponseResolver` in `AspNetCore` to
  understand how outputs and exceptions are mapped to HTTP status codes.
- For authentication issues, check that the appsettings keys used by `TokenProvider` are present and that the
  secret/signing key is the same across services that validate tokens.
- The output wrapper types such as `ProcessOutput` and `DataOutput` (in `ArturRios.Common.Output`) are used in
  controllers and clients — follow the same patterns for consistency.

Where to find source files
-------------------------

- Project root: `src\ArturRios.Common.Web\`
- Middleware: `src\ArturRios.Common.Web\Middleware\`
- HTTP helpers: `src\ArturRios.Common.Web\Http\`
- Security: `src\ArturRios.Common.Web\Security\`
- API helpers and clients: `src\ArturRios.Common.Web\Api\`
