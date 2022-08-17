﻿using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface IItemRepository:IBaseRepository<Item>
    {
        Task<IEnumerable<Item>> GetByCollectionAsync(Guid id);
    }
}
