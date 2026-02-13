// Use environment variable for API URL
const API_BASE_URL = import.meta.env.VITE_API_URL || '/api';

export async function fetchMovie(title) {
    const response = await fetch(`${API_BASE_URL}/MovieAPI?apikey=70ad3b4320b040f862560fa3302f09fc&title=${encodeURIComponent(title)}`);

    if (response.status === 404) throw new Error('Movie not found');
    if (!response.ok) throw new Error(`Request failed: ${response.status}`);

    return response.json();
}
```

**Create `.env.development` in your Vue project root:**
```
VITE_API_URL=/api
```

**Create `.env.production` in your Vue project root:**
```
VITE_API_URL=https://api-site-2sl2.onrender.com/api