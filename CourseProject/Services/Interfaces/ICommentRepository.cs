﻿using CourseProject.Models;

namespace CourseProject.Services.Interfaces
{
    public interface ICommentRepository : IBaseRepository<Comment>
    {
        Task DeleteByUserAsync(string id);
    }
}
