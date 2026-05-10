<template>
  <div class="position-fixed top-0 end-0 h-100 border-start z-3 bg-body width-600">
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
      <div class="mx-2">
        <EditLableComponent v-model="props.task.name"
                            v-on:update="taskUpdate(task)"
                            placeholder="New Task"
                            maxlength="128" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import ManagementTaskService from '@/service/management/task'
  import type { ManagementTaskFull } from '@/service/management/task'

  import EditLableComponent from '@/reusable/edit-lable.vue'

  const emit = defineEmits(['taskDeselect'])

  const props = defineProps<{
    task: ManagementTaskFull
  }>()

  const managementTaskService = new ManagementTaskService()

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
    emit('taskSelect')
  }
</script>

<style scoped>
  .width-600 {
    width: 600px;
  }
</style>