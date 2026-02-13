using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class MovieAPIController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public MovieAPIController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovie(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            return BadRequest("Title is required");

        var apiKey = _configuration["TMDB:ApiKey"];
        var client = _httpClientFactory.CreateClient();

        // Step 1: Search for the movie to get its ID
        var searchUrl = $"https://api.themoviedb.org/3/search/movie?api_key={apiKey}&query={title}";
        var searchResponse = await client.GetAsync(searchUrl);
        if (!searchResponse.IsSuccessStatusCode)
            return StatusCode((int)searchResponse.StatusCode, "Error calling TMDB");

        var searchContent = await searchResponse.Content.ReadAsStringAsync();
        using var searchDoc = JsonDocument.Parse(searchContent);
        var results = searchDoc.RootElement.GetProperty("results");
        if (results.GetArrayLength() == 0)
            return NotFound("Movie not found");

        var movieId = results[0].GetProperty("id").GetInt32();

        // Step 2: Fetch full details + videos using the ID
        var detailUrl = $"https://api.themoviedb.org/3/movie/{movieId}?api_key={apiKey}&append_to_response=videos";
        var detailResponse = await client.GetAsync(detailUrl);
        if (!detailResponse.IsSuccessStatusCode)
            return StatusCode((int)detailResponse.StatusCode, "Error fetching movie details");

        var detailContent = await detailResponse.Content.ReadAsStringAsync();
        using var detailDoc = JsonDocument.Parse(detailContent);
        var movie = detailDoc.RootElement;

        // Poster
        var posterPath = movie.TryGetProperty("poster_path", out var pathProp) && pathProp.ValueKind != JsonValueKind.Null
            ? $"https://image.tmdb.org/t/p/w500{pathProp.GetString()}"
            : null;

        // Genres
        var genres = movie.GetProperty("genres")
            .EnumerateArray()
            .Select(g => g.GetProperty("name").GetString())
            .ToList();

        // Runtime
        var runtime = movie.GetProperty("runtime").GetInt32();
        var runtimeFormatted = $"{runtime / 60}h {runtime % 60}m";

        // Origin country
        var originCountry = movie.TryGetProperty("origin_country", out var oc) && oc.GetArrayLength() > 0
            ? oc[0].GetString()
            : null;

        // Trailer
        string? trailerUrl = null;
        if (movie.TryGetProperty("videos", out var videos))
        {
            var trailer = videos.GetProperty("results")
                .EnumerateArray()
                .FirstOrDefault(v =>
                    v.GetProperty("type").GetString() == "Trailer" &&
                    v.GetProperty("site").GetString() == "YouTube");

            if (trailer.ValueKind != JsonValueKind.Undefined)
            {
                var key = trailer.GetProperty("key").GetString();
                trailerUrl = $"https://www.youtube.com/embed/{key}";
            }
        }

        var cleaned = new
        {
            Title = movie.GetProperty("title").GetString(),
            ReleaseDate = movie.GetProperty("release_date").GetString(),
            OriginCountry = originCountry,
            Genres = genres,
            Runtime = runtimeFormatted,
            Rating = Math.Round(movie.GetProperty("vote_average").GetDouble(), 1, MidpointRounding.AwayFromZero),
            Overview = movie.GetProperty("overview").GetString(),
            PosterUrl = posterPath,
            TrailerUrl = trailerUrl
        };

        return Ok(cleaned);
    }
}