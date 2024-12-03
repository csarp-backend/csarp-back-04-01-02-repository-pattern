namespace Kreta.Shared.Models
{
    public interface IDbEntity<TEntity> where TEntity : class, new()
    {
        public Guid Id { get; set; }
        public bool HasId => Id != Guid.Empty ? true : false;
        public string GetDbSetName()
        {
            string str =new TEntity().GetType().Name;
            return str.EndsWith("s") ? str.Substring(0, str.Length - 1) : str;
        }
    }
}
