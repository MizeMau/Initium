<template>
  <div class="container-fluid mt-3">
    <div class="d-flex flex-nowrap overflow-auto gap-3">
      <div v-for="section in project.sections"
           v-bind:key="section.managementSectionID"
           class="flex-shrink-0 width-300">
        <div class="card border-bottom-0 rounded-0 rounded-top position-relative"
             style="height: calc(100vh - 157px)"
             @mouseover="section.isHover = true" 
             @mouseleave="section.isHover = false">
          <div class="card-header">
            <div class="row">
              <div class="col-10">
                <EditLableComponent v-model="section.name"
                                    v-on:update="sectionNameUpdate(section)"
                                    maxlength="64" />
              </div>
              <div class="col-2 align-content-center">
                <button class="btn btn-outline-primary p-1 px-2"
                        v-on:click="section.tasks.push({})">
                  <i class="bi bi-plus-circle fs-6" />
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
          <button class="btn btn-sm btn-outline-danger position-absolute bottom-0 end-0 m-3"
                  v-on:click="deleteSection(section)">
            <i class="bi bi-trash-fill fs-6" />
          </button>
        </div>
      </div>
      <button class="btn btn-outline-primary h-auto width-300"
              v-on:click="addSection">
        Add section
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
  import EditLableComponent from '@/reusable/edit-lable.vue'

  import ManagementSectionService from '@/service/management/section'
  import type { ManagementSection, ManagementSectionFull } from '@/service/management/section'
  import type { ManagementProjectFull } from '@/service/management/project'

  const props = defineProps<{
    project: ManagementProjectFull
  }>()

  const managementSectionService = new ManagementSectionService()

  function addSection() {
    const section: ManagementSectionFull = {
      managementSectionID: 0,
      created: new Date(0),
      deleted: null,
      name: '',
      managementProjectID: props.project.managementProjectID,
      tasks: []
    }
    props.project.sections.push(section)
  }

  async function sectionNameUpdate(value: ManagementSectionFull) {
    const tmp = { ...value }
    const tmpTasks = value.tasks
    delete (tmp as any).tasks
    var section: ManagementSectionFull | undefined
    if (tmp.managementSectionID == 0)
      section = await managementSectionService.create(tmp)
    else
      section = await managementSectionService.update(tmp)

    section!.tasks = tmpTasks
    value = section!
  }

  async function deleteSection(value: ManagementSectionFull) {
    const check = confirm(`Are you sure you want to delete "${value.name}"?`)
    if (!check)
      return

    if (value.managementSectionID != 0) {
      const success = await managementSectionService.delete('', value.managementSectionID)
      if (!success)
        return
    }

    const index = props.project.sections.findIndex(f => f.managementSectionID == value.managementSectionID)
    props.project.sections.splice(index, 1);
  }
</script>

<style scoped>
  .width-300 {
    width: 300px;
  }
</style>