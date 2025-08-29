using ArturRios.Common.Output;

namespace ArturRios.Common.Attributes.EndpointToggle;

public class EndpointDisabledException(string[]? messages, string message)
    : CustomException(messages ?? [message], message);
