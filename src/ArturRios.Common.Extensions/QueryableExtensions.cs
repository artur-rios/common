using System.Linq.Expressions;
using System.Reflection;
using ArturRios.Common.Output;
using Microsoft.EntityFrameworkCore;

namespace ArturRios.Common.Extensions;

public static class QueryableExtensions
{
    public static async Task<PaginatedOutput<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        string? orderByProperty = null,
        CancellationToken cancellationToken = default)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);

        if (query is not IOrderedQueryable<T>)
        {
            query = OrderByProperty(query, orderByProperty);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        IReadOnlyList<T> items = totalCount == 0
            ? Array.Empty<T>()
            : await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

        return PaginatedOutput<T>.New
            .WithData(items.ToList())
            .WithPagination(pageNumber, pageSize);
    }

    public static PaginatedOutput<T> Paginate<T>(
        this IQueryable<T> query,
        int pageNumber,
        int pageSize,
        string? orderByProperty = null)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, pageSize);

        if (query is not IOrderedQueryable<T>)
        {
            query = OrderByProperty(query, orderByProperty);
        }

        var totalCount = query.Count();

        IReadOnlyList<T> items = totalCount == 0
            ? Array.Empty<T>()
            : query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

        return PaginatedOutput<T>.New
            .WithData(items.ToList())
            .WithPagination(pageNumber, pageSize);
    }

    private static IQueryable<T> OrderByProperty<T>(IQueryable<T> source, string? propertyName)
    {
        var type = typeof(T);
        var candidates = propertyName is not null
            ? new[] { propertyName }
            : new[] { "Id", $"{type.Name}Id" };

        var prop = candidates
            .Select(name => type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase))
            .FirstOrDefault(p => p is not null);

        if (prop is null)
        {
            throw new InvalidOperationException(
                "Pagination requires deterministic ordering. Provide an OrderBy(...) before Paginate or specify 'orderByProperty'.");
        }

        var parameter = Expression.Parameter(type, "e");
        var propertyAccess = Expression.Property(parameter, prop);
        var lambdaType = typeof(Func<,>).MakeGenericType(type, prop.PropertyType);
        var keySelector = Expression.Lambda(lambdaType, propertyAccess, parameter);

        var orderByMethod = typeof(Queryable)
            .GetMethods()
            .First(m => m.Name == "OrderBy" && m.GetParameters().Length == 2)
            .MakeGenericMethod(type, prop.PropertyType);

        return (IQueryable<T>)orderByMethod.Invoke(null, [source, keySelector])!;
    }
}
