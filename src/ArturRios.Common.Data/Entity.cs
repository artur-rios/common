// ReSharper disable UnusedMember.Global
// Reason: This class is meant to be used in other projects

using System.ComponentModel.DataAnnotations.Schema;

namespace ArturRios.Common.Data;

public abstract class Entity
{

    [Column(Order = 1)]
    public int Id { get; set; }
}
