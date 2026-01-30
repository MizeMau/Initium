<template>
  <div class="container-fluid mt-3">
    <div class="d-flex flex-nowrap overflow-auto gap-3">
      <div v-for="section in sections"
           :key="section.managementSectionID"
           class="flex-shrink-0 width-300">
        <div class="card border-bottom-0 rounded-0 rounded-top"
             style="height: calc(100vh - 157px)">
          <div class="card-header">
            <div class="row">
              <div class="col-10">
                <input v-model="section.name"
                       placeholder="New Section"
                       class="form-control border-0 bg-transparent" />
              </div>
              <div class="col-2 align-content-center">
                <button class="btn btn-outline-primary p-1 px-2"
                        v-on:click="section.tasks.push({})">
                  <i class="bi bi-plus-circle fs-6"></i>
                </button>
              </div>
            </div>
          </div>
          <div class="card-body overflow-auto">
            <div v-for="task in section.tasks"
                 :key="task.id"
                 class="mb-2">
              <div class="card">
                {{ task.name }}
              </div>
              </div>
            </div>
        </div>
      </div>
      <button class="btn btn-outline-primary h-auto width-300"
              v-on:click="sections.push({})">
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