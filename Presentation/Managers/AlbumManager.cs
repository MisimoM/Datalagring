using Business.Services;
using Shared.Models.Album;
using System.Diagnostics;

namespace Presentation.Managers
{
    public class AlbumManager(AlbumService albumService)
    {
        private readonly AlbumService _albumService = albumService;

        public async Task ShowAllAlbums()
        {
            try
            {
                var albums = await _albumService.GetAlbumsAsync();

                Console.Clear();
                Console.WriteLine("Showing Albums");
                Console.WriteLine("");

                if (!albums.Any())
                {
                    Console.WriteLine("No albums available.");
                }

                foreach (var album in albums)
                {
                    Console.WriteLine($"Id: {album.Id}, Title: {album.Title}, Artist: {album.Artist}, Price: ${album.Price:F0}");
                    Console.WriteLine("Tracks:");
                    foreach (var track in album.Tracks)
                    {
                        Console.WriteLine($"{track.Title}");
                    }
                    Console.WriteLine("");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async Task AddAlbum()
        {
            try
            {
                Console.WriteLine("Enter details for the new album:");

                Console.Write("Title: ");
                var title = Console.ReadLine();

                Console.Write("Artist: ");
                var artist = Console.ReadLine();

                Console.Write("Price: ");
                if (!decimal.TryParse(Console.ReadLine(), out var price))
                {
                    Console.WriteLine("Invalid price format.");
                    return;
                }

                Console.WriteLine("Enter details for the tracks (press Enter to finish):");
                var trackModels = new List<TrackModel>();

                while (true)
                {
                    Console.Write("Track Title (press Enter to finish): ");
                    var trackTitle = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(trackTitle))
                        break;

                    trackModels.Add(new TrackModel { Title = trackTitle! });
                }

                var newAlbumModel = new AlbumModel
                {
                    Title = title!,
                    Artist = artist!,
                    Price = price,
                };

                var newAlbumEntity = await _albumService.CreateAlbumAsync(newAlbumModel, trackModels);

                if (newAlbumEntity != null)
                {
                    Console.WriteLine($"New album added successfully. Album ID: {newAlbumEntity.Id}");
                }
                else
                {
                    Console.WriteLine("Failed to add the new album.");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
        public async Task RemoveAlbum()
        {
            try
            {
                Console.WriteLine("Enter the ID of the album you want to remove");
                Console.Write("ID number: ");

                if (int.TryParse(Console.ReadLine(), out int input))
                {
                    bool removed = await _albumService.RemoveAlbumAsync(input);

                    if (removed)
                    {
                        Console.WriteLine($"Album with ID {input} successfully removed.");
                    }
                    else
                    {
                        Console.WriteLine($"Unable to remove album with ID {input}.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }

        public async Task UpdateAlbum()
        {
            try
            {
                Console.WriteLine("Enter the ID of the album you want to update");
                Console.Write("ID number: ");

                if (int.TryParse(Console.ReadLine(), out int albumId))
                {
                    Console.WriteLine("Enter updated details for the album:");

                    Console.Write("New Title: ");
                    var newTitle = Console.ReadLine();

                    Console.Write("New Artist: ");
                    var newArtist = Console.ReadLine();

                    Console.Write("New Price: ");
                    if (!decimal.TryParse(Console.ReadLine(), out var newPrice) || newPrice < 0)
                    {
                        Console.WriteLine("Invalid price format. Please enter a valid positive number.");
                        return;
                    }

                    Console.WriteLine("Enter details for the new tracks (press Enter to finish):");
                    var updatedTrackModels = new List<TrackModel>();

                    while (true)
                    {
                        Console.Write("Track Title (press Enter to finish): ");
                        var trackTitle = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(trackTitle))
                            break;

                        updatedTrackModels.Add(new TrackModel { Title = trackTitle! });
                    }

                    var updatedAlbumModel = new AlbumModel
                    {
                        Title = newTitle!,
                        Artist = newArtist!,
                        Price = newPrice,
                    };

                    var updatedAlbumEntity = await _albumService.UpdateAlbumAsync(albumId, updatedAlbumModel, updatedTrackModels);

                    if (updatedAlbumEntity != null)
                    {
                        Console.WriteLine($"Album with ID {albumId} successfully updated.");
                    }
                    else
                    {
                        Console.WriteLine($"Unable to update album with ID {albumId}.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric ID.");
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.Message); }
        }
    }
}
