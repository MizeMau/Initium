<template>
  <div class="position-fixed top-0 end-0 h-100 border-start z-3 bg-body width-600 px-3 overflow-scroll">
    <div>
      <div>
        <button class="btn border-0">
          <i class="bi bi-check2 fs-4" />
        </button>
        <div class="float-end">
          <button class="btn border-0"
                  @click="deselectTask">
            <i class="bi bi-x fs-4" />
          </button>
        </div>
      </div>
      <hr class="mt-0" />
      <div class="row">
        <div class="col-md-12">
          <EditLableComponent v-model="props.task.name"
                              v-on:update="taskUpdate(task)"
                              placeholder="New Task"
                              maxlength="128"
                              v-bind:innerClass="{'px-11': true, 'form-control-lg': true}" />
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-md-3">
          <small class="mt-2 ms-2">
            Assignee
          </small>
        </div>
        <div class="col-md-9">
          <button class="btn btn-outline-secondary btn-sm"
                  title="WIP">
            No assignee
          </button>
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-md-3">
          <small class="mt-2 ms-2">
            Start date
          </small>
        </div>
        <div class="col-md-9">
          <button class="btn btn-outline-secondary btn-sm"
                  title="WIP">
            No start date
          </button>
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-md-3">
          <small class="mt-2 ms-2">
            Due date
          </small>
        </div>
        <div class="col-md-9">
          <button class="btn btn-outline-secondary btn-sm"
                  title="WIP">
            No due date
          </button>
        </div>
      </div>
      <div class="row mt-3">
        <div class="col-md-12">
          Description
          <RichTextEditor v-model="task.description"
                          class="mt-2 mx-2"
                          minHeight="300px"
                          @input="descriptionInput"/>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { useDebounceFn } from "@vueuse/core"

  import ManagementTaskService from '@/service/management/task'
  import type { ManagementTaskFull } from '@/service/management/task'

  import EditLableComponent from '@/reusable/edit-lable.vue'
  import RichTextEditor from '@/reusable/rich-text-editor.vue'

  const emit = defineEmits(['taskDeselect'])

  const props = defineProps<{
    task: ManagementTaskFull
  }>()

  const managementTaskService = new ManagementTaskService()

  const debouncedFn = useDebounceFn(async () => {
    await taskUpdate(props.task)
  }, 1500)

  async function taskUpdate(value: ManagementTaskFull) {
    const tmp = { ...value }
    const tmpTasks = value.tasks
    delete (tmp as any).tasks
    var task: ManagementTaskFull | undefined
    task = await managementTaskService.update(tmp)

    task!.tasks = tmpTasks
    value = task!
  }

  function deselectTask() {
    emit('taskDeselect')
  }

  function descriptionInput(event: any) {
    //props.task.description = event.target.innerHTML
    debouncedFn()
  }
</script>

<style scoped>
  .width-600 {
    width: 600px;
  }
</style>