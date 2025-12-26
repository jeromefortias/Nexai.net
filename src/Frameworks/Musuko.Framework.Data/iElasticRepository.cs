namespace Musuko.Core.Data
{
    using Musuko.Framework.DataModels;
    public interface iElasticRepository<TEntity>
        where TEntity : EntityBase
    {
    }
    public interface IElasticLogRepository : iElasticRepository<Log>
    {
    }
}
