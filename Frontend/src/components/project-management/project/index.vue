<template>
  <div class="container container-lg mt-3">
    <h1>
      Project {{ project?.name }}
    </h1>
    <hr />
    <nav>
      <div class="nav nav-tabs" id="nav-tab" role="tablist">
        <button class="nav-link" id="nav-list-tab" data-bs-toggle="tab" data-bs-target="#nav-list" type="button" role="tab" aria-controls="nav-list" aria-selected="false">List</button>
        <button class="nav-link active" id="nav-board-tab" data-bs-toggle="tab" data-bs-target="#nav-board" type="button" role="tab" aria-controls="nav-board" aria-selected="true">Board</button>
      </div>
    </nav>
    <div v-if="project"
         class="tab-content" id="nav-tabContent">
      <div class="tab-pane fade" id="nav-list" role="tabpanel" aria-labelledby="nav-list-tab">

      </div>
      <div class="tab-pane fade show active" id="nav-board" role="tabpanel" aria-labelledby="nav-board-tab">
        <BoardTabComponent v-bind:project="project" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import { useRoute } from 'vue-router'
  import ManagementProjectService from '@/service/management/project'
  import type { ManagementProjectFull } from '@/service/management/project'

  import BoardTabComponent from './board-tab.vue'

  const route = useRoute()

  const managementProjectService = new ManagementProjectService()

  const project = ref<ManagementProjectFull>()

  onMounted(async () => {
    const managementProjectID: number = +route.params.id
    if (managementProjectID == null) return
    project.value = await managementProjectService.getFullByProject(managementProjectID)
  })
</script>