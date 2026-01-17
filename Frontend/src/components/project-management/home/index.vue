<template>
  <div ref="chartEl" style="width: 100%; height: 100vh;"></div>
</template>

<script setup lang="ts">
  import { ref, onMounted, inject } from 'vue'
  import router from '@/router'
  import ManagementProjectService from '@/service/management/project'
  import type { ManagementProject } from '@/service/management/project'
  import type * as echarts from 'echarts'

  const managementProjectService = new ManagementProjectService()

  const echartsInstance = inject<typeof echarts>('echarts')
  
  const projects = ref<ManagementProject[]>([])
  const chartEl = ref<HTMLDivElement | null>(null)
  const chart = ref<any>(null)

  onMounted(async () => {
    projects.value = await managementProjectService.getAll()
    initChart()
  })


  function initChart() {
    chart.value = echartsInstance!.init(chartEl.value)

    const data = projects.value.map(project => ({
      id: project.managementProjectID,
      name: project.name,
      symbolSize: 100,
      draggable: true,
      itemStyle: {
        color: '#800080'
      }
    }))

    const option = {
      textStyle: {
        fontFamily: 'OpenDyslexic, Fira Code, monospace'
      },
      series: [
        {
          type: 'graph',
          layout: 'force',
          roam: true,
          data,
          edges: [],
          force: {
            repulsion: 400,
            gravity: 0.1,
            edgeLength: 1
          },
          label: {
            show: true,
            formatter: '{b}',
            position: 'inside',
            color: '#fff',
            fontSize: 12
          },
          emphasis: {
            scale: true
          }
        }
      ]
    }

    chart.value.setOption(option)

    chart.value.on('click', (params: any) => {
      if (params.dataType === 'node') {
        onNodeClick(params.data.id)
      }
    })
  }

  function onNodeClick(id: number) {
    console.log(id)
    router.push(`/project-management/project/${id}`)
  }
</script>
