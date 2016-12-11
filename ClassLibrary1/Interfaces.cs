using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFramework;

namespace Interfaces
{
    public interface ITodoRepository
    {
        TodoItem Get(Guid todoId, Guid userId);
        void Add(TodoItem todoItem);
        bool Remove(Guid todoId, Guid userId);
        void Update(TodoItem todoItem, Guid userId);
        bool MarkAsCompleted(Guid todoId, Guid userId);
        List<TodoItem> GetAll(Guid userId);
        List<TodoItem> GetActive(Guid userId);
        List<TodoItem> GetCompleted(Guid userId);
        List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId);
    }
}