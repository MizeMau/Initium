<template>
  <div>
    <div class="row">
      <div ref="editorRef"
           contenteditable="true"
           class="form-control col-md-12"
           v-bind:style="{ 'min-height': minHeight }"
           @keyup="syncButtons"
           @mouseup="syncButtons"
           @input="onInput"/>
    </div>
    <div class="row mt-1">
      <div class="col-md-12 px-0">
        <div class="float-end">
          <div class="btn-group">
            <div class="btn-group"
                 role="toolbar">
              <button v-for="btn in buttons"
                      :key="btn.cmd"
                      type="button"
                      class="btn btn-sm btn-outline-secondary float-end"
                      v-bind:class="{ 'active': activeCommands[btn.cmd] }"
                      @click="fmt(btn.cmd)"
                      v-html="btn.label" />
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, reactive, onMounted, watch } from 'vue'
  const model = defineModel<string>()
  const emit = defineEmits(['input'])

  const props = defineProps<{
    minHeight?: string
  }>()

  const editorRef = ref<HTMLInputElement | null>(null)

  const buttons = [
    { cmd: 'bold', label: '<b>B</b>', title: 'Bold' },
    { cmd: 'italic', label: '<i>I</i>', title: 'Italic' },
    { cmd: 'underline', label: '<u>U</u>', title: 'Underline' },
    { cmd: 'strikeThrough', label: '<s>S</s>', title: 'Strikethrough' },
    { cmd: 'insertUnorderedList', label: '• ≡', title: 'Bullet list' },
    { cmd: 'insertOrderedList', label: '1 ≡', title: 'Numbered list' },
  ]

  const activeCommands = reactive(
    Object.fromEntries(buttons.map(b => [b.cmd, false]))
  )

  onMounted(async () => {
    if (model.value != null && editorRef?.value != null) {
      editorRef.value.innerHTML = model.value
    }
  })

  watch(model, async () => {
    if (model.value != null && editorRef?.value != null && model.value != editorRef.value.innerHTML) {
      editorRef.value.innerHTML = model.value
    }
  })

  function fmt(cmd: string) {
    editorRef.value.focus()
    document.execCommand(cmd, false, null)
    syncButtons()
  }

  function syncButtons() {
    buttons.forEach(({ cmd }) => {
      activeCommands[cmd] = document.queryCommandState(cmd)
    })
  }

  function onInput(event: any) {
    model.value = editorRef.value?.innerHTML ?? ''
    emit('input', event)
  }
</script>