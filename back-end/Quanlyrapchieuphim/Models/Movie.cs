namespace Quanlyrapchieuphim.Models
{
    public class Movie
    {
        public Guid MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string MovieDirector { get; set; }
        public string MovieActors { get; set; }
        public Guid GenreId { get; set; }
        public string GenreName { get; set; }
        public DateTime YearOfRelease { get; set; }
        public int Duration { get; set; }
        public string Image { get; set; }
        public string Language { get; set; }
        public string Summary { get; set; }


    }
}
