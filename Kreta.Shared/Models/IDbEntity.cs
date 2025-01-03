﻿namespace Kreta.Shared.Models
{
    public interface IDbEntity<TEntity> where TEntity : class, new()
    {
        public Guid Id { get; set; }
        public bool HasId => Id != Guid.Empty ? true : false;
        public string GetDbSetName()
        {
            return string.Concat(new TEntity().GetType().Name, 's');
        }
    }
}
