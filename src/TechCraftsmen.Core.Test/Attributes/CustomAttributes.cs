namespace TechCraftsmen.Core.Test.Attributes;

public class UnitAttribute(string[]? environments = null) : CustomFactAttribute(environments);

public class FunctionalAttribute(string[]? environments = null) : CustomFactAttribute(environments);
