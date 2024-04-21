using JWT_Authentication.Models;

namespace JWT_Authentication.Repositories
{
    public class BookRepository
    {
        public static List<Book> books = new()
        {
            new(1, "Clean Code", "R. Martin", 4.5),
            new(2, "Agile", "R. Martin", 5)
        };
    }
}
