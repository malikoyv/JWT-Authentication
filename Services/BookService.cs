using JWT_Authentication.Models;
using JWT_Authentication.Repositories;

namespace JWT_Authentication.Serives
{
    public class BookService : IBookService
    {
        // Endpoints' services of books
        public Book Create(Book newBook)
        {
            newBook.Id = BookRepository.books.Count() + 1;

            BookRepository.books.Add(newBook);
            return newBook;
        }
        public Book Get(int id)
        {
            Book returnBook = BookRepository.books.FirstOrDefault(b => b.Id == id);
            return returnBook;
        }
        public List<Book> GetAll()
        {
            return BookRepository.books;
        }
        public Book Update(Book newBook)
        {
            Book oldBook = BookRepository.books.FirstOrDefault(b => b.Id == newBook.Id);
            if (oldBook == null)
            {
                return null;
            }
            oldBook.author = newBook.author;
            oldBook.title = newBook.title;
            oldBook.rating = newBook.rating;

            return newBook;
        }
        public bool Delete(int id)
        {
            Book book = BookRepository.books.FirstOrDefault(b => b.Id == id);
            if (book == null) { return  false; }
            BookRepository.books.Remove(book);
            return true;
        }
    }
}
