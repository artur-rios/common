using ArturRios.Common.Output;

namespace ArturRios.Common.Util.Waiter;

public class MaxRetriesReachedException(string[]? messages = null)
    : CustomException(messages ?? ["Maximum retry count exceeded"]);
