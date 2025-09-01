# Common

A collection of reusable utilities for .NET projects, designed to be shared across multiple solutions. This library is
structured into modular subprojects, so you can reference only what you need.

I maintain this in my free time, and it's a space where I explore ideas, experiment with patterns, and occasionally
indulge in some overengineering. Contributions are very welcome—feel free to jump in!

## Can I use it in real projects?

Yes, you can. Most features are actively used in my own projects and are tested accordingly. The goal is to achieve full
unit test coverage across the library — while we're not there yet, it's a work in progress.

That said, some components are experimental and may evolve over time. I recommend reviewing the code before integrating
it into production. If you spot something odd or want to improve it, feel free to open an issue, submit a pull request,
or fork the repo and make it your own.

Liked it? Please consider giving a star to the repository.

## Recommended Use

This library is designed to be added as a Git submodule to your .NET solution, making it easy to share and reuse code across multiple projects.

### Add as a Submodule

To include this library in your repository:

```bash
git submodule add https://github.com/artur-rios/common.git common
git submodule update --init --recursive
```

### Reference Only What You Need

The library is split into multiple projects, each targeting a specific set of utilities or patterns. This modular structure allows you to reference only the parts you need:

1. Open your solution in your favorite IDE (e.g., Visual Studio).
2. Add the projects you need as references to your existing projects, by browsing to the submodule folder (e.g., libs/common) and select the specific .csproj files you want to include
3. Build and you're good to go!

## This documentation is under construction

## Features

### Data Utilities

- [Entity](./docs/Entity.md)
- [ICrudRepository](./docs/ICrudRepository.md)
- [DataFilter](./docs/DataFilter.md)

