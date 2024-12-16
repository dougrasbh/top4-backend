namespace BryophytaClassifier.Models;

/// <summary>Generic Entity</summary>
/// <typeparam name="TId"></typeparam>
public abstract class Entity<TId> : IAuditable where TId : struct {
    public TId Id { get; init; }

    public DateTimeOffset CreationTime { get; set; }

    public DateTimeOffset LastUpdateTime { get; set; }

    public virtual bool IsValid() => true;
}

public abstract class EntityFilter<TId, TEntity> 
    where TEntity : Entity<TId>
    where TId : struct, IEquatable<TId> {

    public TId? Id { get; set; }

    public DateTimeOffset? CreationTime { get; set; }

    public DateTimeOffset? LastUpdateTime { get; set; }

    public IEnumerable<TEntity> Apply(IEnumerable<TEntity> entities) =>
        entities.Where(entity =>
            DefaultIsEqual(Id, entity.Id) &&
            DefaultIsEqual(CreationTime, entity.CreationTime) &&
            DefaultIsEqual(LastUpdateTime, entity.LastUpdateTime) &&
            FilterMatches(entity)
        );

    protected abstract bool FilterMatches(TEntity entity);

    protected static bool DefaultIsEqual<T>(T? p1, T? p2) =>
        p1 == null || p2 == null || p1.Equals(p2);

    protected static bool DefaultContains<T>(IEnumerable<T>? p1, T? p2) =>
        p1 == null || p2 == null || p1.Contains(p2);
}