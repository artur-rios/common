// ReSharper disable UnusedType.Global
// Reason: These annotations are meant to be used in other projects

using TechCraftsmen.Core.Environment;

namespace TechCraftsmen.Core.Test.Attributes;

public class UnitFactAttribute(EnvironmentType[]? environments = null, bool skipCondition = false) : CustomFactAttribute(environments, skipCondition);

public class UnitTheoryAttribute(EnvironmentType[]? environments = null, bool skipCondition = false) : CustomTheoryAttribute(environments, skipCondition);

public class FunctionalFactAttribute(EnvironmentType[]? environments = null, bool skipCondition = false) : CustomFactAttribute(environments, skipCondition);

public class FunctionalTheoryAttribute(EnvironmentType[]? environments = null, bool skipCondition = false) : CustomTheoryAttribute(environments, skipCondition);
