<template>
  <div ref="chart" style="width: 100%; height: 100vh;"></div>
</template>

<script>
  import ManagementProjectService from "@/service/management/project"

  export default {
    data() {
      return {
        managementProjectService: new ManagementProjectService(),
        projects: [],
        chart: null
      }
    },
    async mounted() {
      this.projects = await this.managementProjectService.getAll('', {})
      this.initChart()
    },
    methods: {
      initChart() {
        this.chart = this.$echarts.init(this.$refs.chart)

        const data = this.projects.map((project, index) => ({
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
        this.chart.setOption(option)
        this.chart.on('click', params => {
          if (params.dataType === 'node') {
            this.onNodeClick(params.data.id)
          }
        })
      },
      onNodeClick(id) {
        console.log(id)
      },
    }
  }
</script>
