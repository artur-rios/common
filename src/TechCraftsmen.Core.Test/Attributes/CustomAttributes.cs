// ReSharper disable UnusedType.Global
// Reason: These annotations are meant to be used in other projects

using TechCraftsmen.Core.Environment;

namespace TechCraftsmen.Core.Test.Attributes;

public class UnitAttribute(EnvironmentType[]? environments = null) : CustomFactAttribute(environments);

public class FunctionalAttribute(EnvironmentType[]? environments = null) : CustomFactAttribute(environments);
