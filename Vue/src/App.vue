<script setup lang="ts">
import NavBar from './components/navbar.vue';
import Button from './components/Button.vue';
import Homepage from './components/Homepage.vue';
import { ref } from 'vue'
import ContactPage from './components/ContactPage.vue';
import MoviePage from './components/MoviePage.vue';

let currentComponent = ref('HomePage');

function changeComponent(newComponent: string) {
  currentComponent.value = newComponent;
}
</script>
<template>
  <div class="container eb-garamond">
    <NavBar>
      <template #left>
        <Button :buttonclicked="() => changeComponent('HomePage')"
          :imgurl="'https://i.kym-cdn.com/entries/icons/facebook/000/016/232/House.jpg'">
        </Button>
      </template>
      <template #center>
        <Button :buttonclicked="() => changeComponent('MoviePage')" :text="'Movies'"></Button>
      </template>
      <template #right>
        <Button :buttonclicked="() => changeComponent('ContactPage')" :text="'Contact'"></Button>
      </template>
    </NavBar>

    <Homepage v-if="currentComponent == 'HomePage'"></Homepage>
    <ContactPage v-if="currentComponent == 'ContactPage'"></ContactPage>
    <MoviePage v-if="currentComponent == 'MoviePage'"></MoviePage>
  </div>
</template>