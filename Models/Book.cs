namespace JWT_Authentication.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string title {  get; set; }
        public string author { get; set; }
        public double rating { get; set; }

        public Book(int id, string title, string author, double rating)
        {
            Id = id;
            this.title = title;
            this.author = author;
            this.rating = rating;
        }
    }
}
