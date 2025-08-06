using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Waiter;

public class MaxRetriesReachedException(string[]? messages = null, string message = "Internal error")
    : CustomException(messages ?? ["Maximum retry count exceeded"], message);
