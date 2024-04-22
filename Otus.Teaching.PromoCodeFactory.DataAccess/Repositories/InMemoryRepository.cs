using Otus.Teaching.PromoCodeFactory.Core.Abstractions.Repositories;
using Otus.Teaching.PromoCodeFactory.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T>
        : IRepository<T>
        where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }


        public Task<T> CreateAsync(T entity)
        {
            entity.Id = Guid.NewGuid();
            var dataList = Data.ToList();
            dataList.Add(entity);
            Data = dataList;
            return Task.FromResult(entity);

        }

        public Task<T> Delete(Guid id)
        {
            T entity = Data.FirstOrDefault(x => x.Id == id);
            Data = Data.Where(x => x.Id != id);
            return Task.FromResult(entity);
        }

        public Task<T> Update(T entity)
        {
            var dataList = Data.ToList();
            var data = dataList.FirstOrDefault(x => x.Id == entity.Id);

            if (data == null)
                return null;

            int index = dataList.IndexOf(data);
            if (index == -1)
            {
                return null;
            }

            dataList[index] = entity;

            Data = dataList;           
            return Task.FromResult(entity);
        }
    }
}