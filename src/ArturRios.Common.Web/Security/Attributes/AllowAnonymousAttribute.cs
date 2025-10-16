// ReSharper disable ClassNeverInstantiated.Global
// Reason: This class is used as an attribute and is not instantiated directly

namespace ArturRios.Common.Web.Security.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute;
