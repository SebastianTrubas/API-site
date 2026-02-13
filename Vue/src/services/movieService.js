export async function fetchMovie(title) {
    const response = await fetch(`/api/MovieAPI?apikey=70ad3b4320b040f862560fa3302f09fc&title=${encodeURIComponent(title)}`);

    if (response.status === 404) throw new Error('Movie not found');
    if (!response.ok) throw new Error(`Request failed: ${response.status}`);

    return response.json();
}