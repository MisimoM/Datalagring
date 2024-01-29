namespace Shared.Models.Album
{
    public class AlbumModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        public decimal Price { get; set; }
        public string Artist { get; set; } = null!;

        public List<TrackModel> Tracks { get; set; } = new List<TrackModel>();

    }
}