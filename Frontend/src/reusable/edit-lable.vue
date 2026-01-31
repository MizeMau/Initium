<style scoped>
  .pointer {
    cursor: pointer;
  }
</style>

<template>
  <div class="inline-editor"
       @dblclick="enableEdit">
    <input ref="inputEl"
           v-model="model"
           v-bind:maxlength="maxlength"
           v-bind:readonly="!edit"
           class="form-control"
           v-bind:class="{'border-0 bg-transparent pointer': !edit}"
           placeholder="New Section"
           @mousedown="blockFocus"
           @blur="disableEdit()" 
           @keydown.enter="disableEdit()"
           @keydown.esc="disableEdit(true)" />
  </div>
</template>

<script setup lang="ts">
  import { ref, nextTick } from 'vue'
  const emit = defineEmits(['update'])

  const model = defineModel<string>()

  defineProps<{
    maxlength?: number | string
  }>()

  const oldModel = ref<string | undefined>('')
  const edit = ref<boolean>(false)
  const inputEl = ref<HTMLInputElement | null>(null)

  function blockFocus(e: MouseEvent) {
    if (!edit.value) {
      e.preventDefault()
    }
  }

  function enableEdit() {
    edit.value = true
    oldModel.value = model.value
    nextTick(() => {
      inputEl.value?.focus()
      //inputEl.value?.select()
    })
  }

  function disableEdit(reset: boolean = false) {
    if (!edit.value)
      return
    edit.value = false

    nextTick(() => {
      inputEl.value?.blur()
    })

    if (reset) {
      model.value = oldModel.value
      return
    }

    if (model.value == oldModel.value)
      return

    emit('update')

  }
</script>