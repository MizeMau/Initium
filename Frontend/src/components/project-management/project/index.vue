<template>
  <div class="container mt-3">
    <h1>
      Project {{ project.name }}
    </h1>
    <hr />
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import ManagementProjectService from '@/service/management/project'
  import type { ManagementProject } from '@/service/management/project'

  const route = useRoute()

  const managementProjectService = new ManagementProjectService()

  const project = ref<ManagementProject>({})

  onMounted(async () => {
    const managementProjectID: number = +route.params.id
    if (managementProjectID != null) {
      project.value = await managementProjectService.getById(managementProjectID)
    }
  })
</script>