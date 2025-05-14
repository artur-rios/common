using TechCraftsmen.Core.Environment;

namespace TechCraftsmen.Core.Test.Attributes;

public class UnitAttribute(EnvironmentType[]? environments = null) : CustomFactAttribute(environments);

public class FunctionalAttribute(EnvironmentType[]? environments = null) : CustomFactAttribute(environments);
