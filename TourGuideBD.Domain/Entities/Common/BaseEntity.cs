namespace TourGuideBD.Domain.Entities.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
}

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;
}