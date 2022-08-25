namespace LastTradeProject.Entities.Common
{
    public abstract class BaseEntity<TKeyType> : IBaseEntity
    {
        public TKeyType Id { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>, IBaseEntity
    {
    }

    public interface IBaseEntity
    {
    }
}
