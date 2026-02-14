<template>
  <div class="container-fluid mt-3">
    <div class="d-flex flex-nowrap overflow-auto gap-3">
      <draggable v-model="project.sections"
                 item-key="managementSectionID"
                 direction="horizontal"
                 class="d-flex flex-nowrap overflow-auto gap-3"
                 handle=".section-handle"
                 @end="onSectionReorder">
        <template #item="{ element: section  }">
          <div class="card border-bottom-0 rounded-0 rounded-top position-relative flex-shrink-0 width-300"
               style="height: calc(100vh - 157px)">
            <div class="card-header">
              <div class="row">
                <div class="col-1 align-content-center">
                  <i class="bi bi-grip-vertical section-handle cursor-grab" />
                </div>
                <div class="col-9">
                  <EditLableComponent v-model="section.name"
                                      v-on:update="sectionUpdate(section)"
                                      placeholder="New Section"
                                      maxlength="64" />
                </div>
                <div class="col-2 align-content-center">
                  <button class="btn btn-outline-primary p-1 px-2"
                          v-on:click="addTask(section)">
                    <i class="bi bi-plus-circle fs-6" />
                  </button>
                </div>
              </div>
            </div>
            <div class="card-body overflow-auto">
              <draggable v-model="section.tasks"
                         item-key="managementTaskID"
                         group="tasks"
                         handle=".task-handle"
                         @end="onTaskDrop($event, section)">
                <template #item="{ element: task }">
                  <div v-if="showCompleted  || !task.isCompleted"
                       class="card container task-handle cursor-grab mb-2">
                    <div class="row">
                      <div class="col-2 pe-0 w-auto mt-1">
                        <i v-if="!task.isCompleted"
                           class="bi bi-check-circle fs-6"
                           v-bind:class="task.isHoverDone ? 'text-success' : 'text-secondary'"
                           v-on:mouseover="task.isHoverDone = true"
                           v-on:mouseleave="task.isHoverDone = false"
                           v-on:click="changeTaskCompleted(task, true)" />
                        <i v-else
                           class="bi bi-check-circle-fill fs-6 text-success"
                           v-on:click="changeTaskCompleted(task, false)" />
                      </div>
                      <div class="col-10 px-0 mt-1 ms-2 mb-1">
                        <div v-if="task.name">
                          {{task.name}}
                        </div>
                        <div v-else class="text-secondary">
                          New Task
                        </div>
                      </div>
                    </div>
                    <div class="row my-2">
                      <div class="col-1">
                        <button class="bg-transparent border rounded-circle text-secondary small"
                                title="Assign Person">
                          <i class="bi bi-person" />
                        </button>
                      </div>
                      <div class="col-1 ms-2">
                        <button class="bg-transparent border rounded-circle text-secondary small"
                                title="Add due date">
                          <i class="bi bi-calendar4" />
                        </button>
                      </div>
                    </div>
                    <template v-if="task.tasks.length > 0"
                              class="table table-sm">
                      <div v-for="subTask in task.tasks">
                        <hr class="m-0" />
                        <div class="row">
                          <div class="col-2 align-content-center pe-0 w-auto">
                            <i v-if="!subTask.isCompleted"
                               class="bi bi-check-circle fs-6"
                               v-bind:class="subTask.isHoverDone ? 'text-success' : 'text-secondary'"
                               v-on:mouseover="subTask.isHoverDone = true"
                               v-on:mouseleave="subTask.isHoverDone = false"
                               v-on:click="changeTaskCompleted(subTask, true)" />
                            <i v-else
                               class="bi bi-check-circle-fill fs-6 text-success"
                               v-on:click="changeTaskCompleted(subTask, false)" />
                          </div>
                          <div class="col-10 px-0">
                            <EditLableComponent v-model="subTask.name"
                                                v-on:update="taskUpdate(subTask)"
                                                placeholder="New Task"
                                                maxlength="128" />
                          </div>
                        </div>
                      </div>
                    </template>
                  </div>
                </template>
              </draggable>
            </div>
            <button class="btn btn-sm btn-outline-danger position-absolute bottom-0 end-0 m-3"
                    v-on:click="deleteSection(section)">
              <i class="bi bi-trash-fill fs-6" />
            </button>
          </div>
        </template>
        <template #footer>
          <button class="btn btn-outline-primary h-auto flex-shrink-0 width-300"
                  @click="addSection">
            Add section
          </button>
        </template>
      </draggable>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref } from 'vue'
  import draggable from 'vuedraggable'

  import EditLableComponent from '@/reusable/edit-lable.vue'

  import ManagementSectionService from '@/service/management/section'
  import type { ManagementSection, ManagementSectionFull } from '@/service/management/section'
  import type { ManagementProjectFull } from '@/service/management/project'
  import ManagementTaskService from '@/service/management/task'
  import type { ManagementTask, ManagementTaskFull } from '@/service/management/task'

  const props = defineProps<{
    project: ManagementProjectFull
  }>()

  const managementSectionService = new ManagementSectionService()
  const managementTaskService = new ManagementTaskService()
  const showCompleted = ref<boolean>(true)

  async function addSection() {
    const section: ManagementSectionFull = {
      managementSectionID: 0,
      created: new Date(0),
      deleted: null,
      name: '',
      managementProjectID: props.project.managementProjectID,
      tasks: [],
    }
    const dbsection = await managementSectionService.create(section)
    props.project.sections.push(dbsection)
  }

  async function sectionUpdate(value: ManagementSectionFull) {
    const tmp = { ...value }
    const tmpTasks = value.tasks
    delete (tmp as any).tasks
    var section: ManagementSectionFull | undefined
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

  async function addTask(section: ManagementSectionFull) {
    const task: ManagementTask = {
      managementTaskID: 0,
      created: new Date(0),
      deleted: null,
      name: '',
      description: '',
      isCompleted: false,
      isMilestone: false,
      start: null,
      end: null,
      managementSectionID: section.managementSectionID,
      managementProjectID: props.project.managementProjectID,
    }
    const dbtask = await managementTaskService.create(task) as ManagementTaskFull
    dbtask.tasks = []
    console.log('dbtask', dbtask)
    section.tasks.push(dbtask)
  }

  async function changeTaskCompleted(task: ManagementTaskFull, value: boolean) {
    task.isCompleted = value
    await taskUpdate(task)
  }

  async function taskUpdate(value: ManagementTaskFull) {
    const tmp = { ...value }
    const tmpTasks = value.tasks
    delete (tmp as any).tasks
    var task: ManagementTaskFull | undefined
    task = await managementTaskService.update(tmp)

    task!.tasks = tmpTasks
    value = task!
  }

  async function onSectionReorder() {
    // project.sections is already reordered
    // Persist order if your backend supports ordering
    console.log('Sections reordered', props.project.sections)
  }

  async function onTaskDrop(event: any) {
    console.log('section', event.from)

    const movedTask: ManagementTaskFull = event.item.__draggable_context.element

    const oldSectionID = movedTask.managementSectionID
    const newSection = props.project.sections.find(section =>
      section.tasks.includes(movedTask)
    )
    if (!newSection) return
    const newSectionID = newSection.managementSectionID

    if (oldSectionID !== newSectionID) {
      movedTask.managementSectionID = newSectionID
      await managementTaskService.updateSectionID(movedTask.managementTaskID, newSectionID)
    }

    var index = newSection.tasks.findIndex(f => f.managementTaskID == movedTask.managementTaskID)
    await managementTaskService.updateSortNumber(movedTask, index)
  }
</script>

<style scoped>
  .width-300 {
    width: 300px !important;
  }
  .cursor-grab {
    cursor: grab;
  }
  .sortable-ghost {
    opacity: 0.4;
  }
</style>