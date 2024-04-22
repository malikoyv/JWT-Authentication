using JWT_Authentication.Models;

namespace JWT_Authentication.Serives
{
    public interface IBookService
    {
        public Book Create(Book newBook);
        public Book Get(int id);
        public List<Book> GetAll();
        public Book Update(Book BookToUpdate);
        public bool Delete(int id);
    }
}
