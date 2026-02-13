<template>
    <img ref="imgRef" :src="src" class="dynamic-image" crossorigin="anonymous" @load="applyShadow" />
</template>

<script setup>
import { ref, onMounted, watch } from "vue"
import ColorThief from "colorthief"

const props = defineProps({
    src: String
})

const imgRef = ref(null)
const colorThief = new ColorThief()

const applyShadow = () => {
    const img = imgRef.value
    if (!img) return

    const color = colorThief.getColor(img)
    const [r, g, b] = color

    img.style.boxShadow = `0 20px 40px rgb(${r}, ${g}, ${b})`
}

onMounted(() => {
    if (imgRef.value.complete) {
        applyShadow()
    }
})

watch(() => props.src, () => {
    setTimeout(() => {
        if (imgRef.value.complete) {
            applyShadow()
        }
    }, 50)
})
</script>
