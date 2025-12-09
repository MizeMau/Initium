<template>
  <div>
    <ol>
      <li v-for="project in projects">{{project.name}}</li>
    </ol>
  </div>
</template>

<script>
  import ManagementProjectService from "@/service/management/project"

  import { useDebounceFn } from "@vueuse/core"

  export default {
    async mounted() {
      this.projects = await this.managementProjectService.getAll('', {})
    },
    data() {
      return {
        managementProjectService: new ManagementProjectService(),
        projects: [],
      }
    },
    methods: {

    },
  }
</script>