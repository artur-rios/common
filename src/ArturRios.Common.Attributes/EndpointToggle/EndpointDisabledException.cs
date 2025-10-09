using ArturRios.Common.Output;

namespace ArturRios.Common.Attributes.EndpointToggle;

public class EndpointDisabledException(string[] messages) : CustomException(messages);
