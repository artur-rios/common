using ArturRios.Common.Pipelines.Commands.IO;

namespace ArturRios.Common.Pipelines.Commands.Interfaces;

public interface ICommand<TOutput> where TOutput : CommandOutput;

public interface ICommand<TInput, TOutput> where TInput : CommandInput where TOutput : CommandOutput;
