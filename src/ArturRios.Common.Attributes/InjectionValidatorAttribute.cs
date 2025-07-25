using Microsoft.Extensions.DependencyInjection;

namespace ArturRios.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class InjectionValidatorAttribute(ServiceLifetime serviceLifetime) : Attribute
{
    public ServiceLifetime ServiceLifetime { get; } = serviceLifetime;
}
