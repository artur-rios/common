namespace ArturRios.Common.Data.Interfaces;

public interface IMultiRepository<out T> where T : Entity
{
    IQueryable<T> GetByMultiFilter(DataFilter filter, bool track = false);
    IEnumerable<T> MultiDelete(IEnumerable<int> ids);
}
