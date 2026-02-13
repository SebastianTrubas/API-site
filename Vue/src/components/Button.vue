<script setup lang="ts">

const props = defineProps<{
    buttonclicked: () => void,
    text?: string,
    imgurl?: string;
}>()
</script>

<template>  
    <button @click="props.buttonclicked"><img v-if="imgurl" :src="imgurl">{{ props.text }}</button>
</template>