﻿// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable NotAccessedPositionalProperty.Global
// Reason: This record is meant to be used in other projects

namespace ArturRios.Common.WebApi.Security.Records;

public record AuthenticatedUser(int Id, int Role);
