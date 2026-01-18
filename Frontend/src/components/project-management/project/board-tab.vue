<template>
  <div class="container-fluid mt-3">
    <div class="d-flex flex-nowrap overflow-auto gap-3">
      <div v-for="section in sections"
           :key="section.managementSectionID"
           class="flex-shrink-0 width-300">
        <div class="card border-bottom-0 rounded-0 rounded-top"
             style="height: calc(100vh - 157px)">
          <div class="card-header">
            {{ section.name }}
          </div>
          <div class="card-body overflow-auto">
            <div v-for="task in section.tasks"
                 :key="task.id"
                 class="mb-2">
              {{ task.title }}
            </div>
          </div>
        </div>
      </div>
      <button class="btn btn-outline-primary h-auto width-300">
        Add section
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
  import type { ManagementSection } from '@/service/management/section'

  defineProps<{
    sections: ManagementSection[]
  }>()
</script>

<style scoped>
  .width-300 {
    width: 300px;
  }
</style>