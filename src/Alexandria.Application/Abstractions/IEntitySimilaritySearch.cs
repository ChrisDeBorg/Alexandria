namespace Alexandria.Application.Abstractions;

public sealed record EntityMatch(Guid EntityId, string Name, double Similarity);

public interface IEntitySimilaritySearch
{
    Task<IReadOnlyList<EntityMatch>> FindSimilarAsync(
        ReadOnlyMemory<float> embedding,
        int limit,
        CancellationToken cancellationToken = default);
}
