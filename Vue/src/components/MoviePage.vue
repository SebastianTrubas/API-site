<script setup lang="ts">
import { ref, computed } from 'vue';
import { fetchMovie } from '../services/movieService';
import ImageCard from './ImageCard.vue';


const query = ref('')
const movie = ref(null)
const error = ref(null)
const loading = ref(false)

const showdata = computed(() => movie.value !== null);

async function search() {
    if (!query.value.trim()) return

    loading.value = true
    error.value = null
    movie.value = null

    try {
        movie.value = await fetchMovie(query.value)
    } catch (err) {
        error.value = err.message
    } finally {
        loading.value = false
    }
}
</script>

<template>
    <div class="container">
        <h1 class="almendra-display">
            Movie Page
        </h1>

        <div class="errors">
            <h2>{{ error }}</h2>
        </div>

        <div class="searchbar-box">
            <input v-model="query" placeholder="Search for a movie..." @keyup.enter="search"
                class="searchbar eb-garamond" />
            <button @click="search" :disabled="loading">
                {{ loading ? 'Searching...' : 'Search' }}
            </button>
        </div>

        <div class="detail-box ">
            <div class="image-box">
                <div class="image-screen" v-if="showdata">
                    <ImageCard :src="movie.posterUrl"></ImageCard>
                </div>
            </div>

            <div class="moviedata-box">
                <div class="moviedata-list" v-if="showdata">
                    <table>
                        <tr>
                            <th>
                                <h2 class="movie-title luxurious-roman">{{ movie.title }}</h2>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                <ul class="movie-metadata">
                                    <li>{{ movie.releaseDate }}</li>
                                    <li>{{ movie.originCountry }}</li>
                                    <li>{{ movie.runtime }}</li>
                                </ul>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                <h3 class="movie-rating">⭐{{ movie.rating }}/10</h3>
                            </th>
                        </tr>
                        <tr>
                            <th>
                                <ul class="genre-pills">
                                    <li v-for="genre in movie.genres" :key="genre" class="genre-pill">
                                        {{ genre }}
                                    </li>
                                </ul>
                            </th>
                        </tr>
                        <tr>
                            <h3 class="movie-trailer">
                                <a :href="movie.trailerUrl" target="_blank">▶︎ Watch Trailer</a>
                            </h3>
                        </tr>
                        <tr>
                            <h3 class="movie-summary">{{ movie.overview }}</h3>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</template>