<template>
  <div class="row">
    <div class="col-md-6">
      <div class="card text-center">
        <h3 class="card-header">
          {{month.name}}
        </h3>
        <div class="card-body">
          <table class="table mt-2">
            <thead>
              <tr>
                <th class="bg-transparent">Mo.</th>
                <th class="bg-transparent">Tu.</th>
                <th class="bg-transparent">We.</th>
                <th class="bg-transparent">Th.</th>
                <th class="bg-transparent">Fr.</th>
                <th class="bg-transparent">Sa.</th>
                <th class="bg-transparent">Su.</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="week in month.weekCount">
                <template v-if="week == 1">
                  <td v-for="offset in month.weekdayOffset"
                      class="bg-transparent" />
                </template>
                <td v-for="day in getWeekDaysByMonthAndWeek(month, week - 1)"
                    v-bind:class="day.isToday ? 'bg-primary' : ''">
                  {{day.number}}
                </td>
                <template v-if="week == month.weekCount">
                  <td v-for="offset in getExtraTd(month)" />
                </template>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <button class="btn btn-secondary mt-4"
              v-on:click="emitExit">
        <i class="bi bi-chevron-double-left" />
        Year overview
      </button>
    </div>
    <div class="col-md-6">
      <div class="card">
        <h3 class="card-header">
          Tasks
        </h3>
        <div class="card-body">

        </div>
      </div>
    </div>
  </div>
</template>

<script>
  export default {
    name: "Month",
    props: ['month'],
    emits: ['exit'],
    data() {
      return {

      };
    },
    mounted() {

    },
    methods: {
      getWeekDaysByMonthAndWeek(month, week) {
        const index = week * 7 - month.weekdayOffset
        const startIndex = week != 0 ? index : 0
        const endIndex = index + 7
        return month.days.slice(startIndex, endIndex)
      },
      getExtraTd(month) {
        var tmp = 7 - (month.weekdayOffset + month.days.length) % 7
        return tmp == 7 ? 0 : tmp
      },
      emitExit() {
        this.$emit('exit')
      },
    },
  }
</script>