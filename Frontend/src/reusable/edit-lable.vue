<style scoped>
  textarea {
    resize: none;
    overflow: hidden;
  }
</style>

<template>
  <div class="inline-editor"
       @dblclick="enableEdit">
    <textarea ref="inputEl"
              rows="1"
              v-model="model"
              v-bind:maxlength="maxlength"
              v-bind:readonly="!edit"
              class="form-control"
              v-bind:class="{'border-0 bg-transparent cursor-pointer': !edit}"
              v-bind:placeholder="placeholder"
              @mousedown="blockFocus"
              @blur="disableEdit()" 
              @keydown.enter="disableEdit()"
              @keydown.esc="disableEdit(true)"
              @input="autoResize" />
  </div>
</template>

<script setup lang="ts">
  import { ref, nextTick, onMounted } from 'vue'
  const emit = defineEmits(['update'])

  const model = defineModel<string>()

  defineProps<{
    maxlength?: number | string
    placeholder?: string
  }>()

  const oldModel = ref<string | undefined>('')
  const edit = ref<boolean>(false)
  const inputEl = ref<HTMLInputElement | null>(null)

  onMounted(async () => {
    autoResize()
  })

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
      autoResize()
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

  function autoResize() {
    const el = inputEl.value
    if (!el) return

    el.style.height = 'auto'
    el.style.height = `${el.scrollHeight}px`
  }
</script>