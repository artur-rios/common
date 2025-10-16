namespace ArturRios.Common.Data.Interfaces;

public interface IRangeRepository<T> where T : Entity
{
    IEnumerable<T> UpdateRange(List<T> entities);
    IEnumerable<int> DeleteRange(List<int> ids);
}
