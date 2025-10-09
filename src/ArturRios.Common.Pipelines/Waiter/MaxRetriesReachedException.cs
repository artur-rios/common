using ArturRios.Common.Output;

namespace ArturRios.Common.Pipelines.Waiter;

public class MaxRetriesReachedException(string[]? messages = null)
    : CustomException(messages ?? ["Maximum retry count exceeded"]);
