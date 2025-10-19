using System.ComponentModel.DataAnnotations.Schema;

namespace ArturRios.Common.Data;

public abstract class Entity
{

    [Column(Order = 1)]
    public int Id { get; set; }
}
