namespace TechCraftsmen.Core.Test.Attributes;

public class FactAttribute(string type = "Unit", string[]? environments = null) : CustomFactAttribute(type, environments);

public class TheoryAttribute(string type = "Unit", string[]? environments = null) : CustomFactAttribute(type, environments);
