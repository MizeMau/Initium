<template>
  <div>
    <router-view />

    <!--<div v-if="loading">Loading...</div>
    <div v-else-if="error">Error: {{ error }}</div>
    <div v-else>
      <h2>API Response:</h2>
      <pre>{{ data }}</pre>
    </div>-->
  </div>
</template>

<script setup>
  import { ref, onMounted } from "vue"
  import axios from "axios"

  const data = ref(null)
  const error = ref(null)
  const loading = ref(true)

  onMounted(async () => {
    try {
      const res = await axios.get("http://localhost:5045/weatherforecast")
      data.value = res.data
    } catch (err) {
      error.value = err.message
    } finally {
      loading.value = false
    }
  })
</script>
