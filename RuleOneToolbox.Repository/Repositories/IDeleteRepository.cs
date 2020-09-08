using System;
using System.Collections.Generic;
using System.Text;

namespace RuleOneToolbox.Repository.Repositories
{
    public interface IDeleteRepository<T> where T : class
    {
        void Delete(T entity);

        void Delete(params T[] entities);

        void Delete(IEnumerable<T> entities);
    }
}
