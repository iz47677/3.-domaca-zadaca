using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces;

namespace EntityFramework
{
    public class TodoSqlRepository : ITodoRepository
    {
        private readonly TodoDbContext _context;

        public TodoSqlRepository(TodoDbContext context)
        {
            _context = context;
        }

        public TodoItem Get(Guid todoId, Guid userId)
        {
            TodoItem result = _context.TodoItems.Where(i => i.Id == todoId).FirstOrDefault();
            if (result?.UserId != userId)
                throw new TodoAccessDeniedException("Access denied: User has no authority to access given item.");
            return result;
        }

        public void Add(TodoItem todoItem)
        {
            if (_context.TodoItems.Any(s => s.Id == todoItem.Id))
                throw new DuplicateTodoItemException(String.Format("duplicate id: {0}", todoItem.Id));
            else
                _context.TodoItems.Add(todoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid todoId, Guid userId)
        {
            TodoItem result = this.Get(todoId, userId);
            if (result != null)
            {
                _context.TodoItems.Remove(result);
                return true;
            }
            return false;
        }

        public void Update(TodoItem todoItem, Guid userId)
        {
            this.Remove(todoItem.Id, userId);
            this.Add(todoItem);
        }

        public bool MarkAsCompleted(Guid todoId, Guid userId)
        {
            if (this.Get(todoId, userId) == null)
                return false;
            _context.TodoItems.Where(i => i.Id == todoId).First().MarkAsCompleted();
            _context.SaveChanges();
            return true;
        }

        public List<TodoItem> GetAll(Guid userId)
        {
            return _context.TodoItems.Where(i => i.UserId == userId).OrderByDescending(i => i.DateCreated).ToList();
        }

        public List<TodoItem> GetActive(Guid userId)
        {
            return _context.TodoItems.Where(i => i.IsCompleted == false && 
                i.UserId == userId).OrderByDescending(i => i.DateCreated).ToList();
        }

        public List<TodoItem> GetCompleted(Guid userId)
        {
            return _context.TodoItems.Where(i => i.IsCompleted == true && 
                i.UserId == userId).OrderByDescending(i => i.DateCreated).ToList();
        }

        public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction, Guid userId)
        {
            return _context.TodoItems.Where(i => filterFunction(i) == true && 
                i.UserId == userId).OrderByDescending(i => i.DateCreated).ToList();
        }
    }

    public class TodoAccessDeniedException : Exception
    {
        public TodoAccessDeniedException(string message) : base(message)
        { }
    }

    public class DuplicateTodoItemException : Exception
    {
        public DuplicateTodoItemException(string message) : base(message)
        { }
    }
}
