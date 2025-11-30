<template>
  <div class="mt-5">
    <div v-if="!selectedMonth"
         class="row">
      <div v-for="month in calendar"
           class="col-xl-3 my-3">
        <div class="card text-center"
             role="button"
             v-on:click="selectedMonth = month">
          <h3 class="card-header">{{month.name}}</h3>
          <div class="card-body">
            <table class="table table-sm">
              <thead>
                <tr>
                  <th>Mo.</th>
                  <th>Tu.</th>
                  <th>We.</th>
                  <th>Th.</th>
                  <th>Fr.</th>
                  <th>Sa.</th>
                  <th>Su.</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="week in month.weekCount">
                  <template v-if="week == 1">
                    <td v-for="offset in month.weekdayOffset" />
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
      </div>
    </div>
    <month-component v-else 
                     v-bind:month="selectedMonth" 
                     v-on:exit="selectedMonth = null" />
  </div>
</template>

<script>
  import MonthComponent from './month.vue'

  export default {
    name: "Calendar",
    components: {
      'month-component': MonthComponent
    },
    data() {
      return {
        today: new Date(),
        calendar: [],
        selectedMonth: null,
      };
    },
    mounted() {
      var monthNames = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
      for (let i = 0; i < 12; i++) {
        let month = {
          name: monthNames[i],
          weekCount: 0,
          weekdayOffset: 0,
          days: [],
        }
        const dateFirst = new Date(this.today.getFullYear(), i, 1)
        const dateLast = new Date(this.today.getFullYear(), i + 1, 0)
        const monthDays = dateLast.getDate()
        let weekdayIndex = dateFirst.getDay()
        month.weekdayOffset = weekdayIndex != 0 ? weekdayIndex - 1 : 6

        if (month.weekdayOffset > 0) month.weekCount++

        for (let y = 0; y < monthDays; y++) {
          month.days.push({
            number: y + 1
          })
          if ((y + month.weekdayOffset) % 7 == 0) month.weekCount++
        }
        this.calendar.push(month) 
      }

      const monthIndex = this.today.getMonth()
      const dayIndex = this.today.getDate() - 1
      this.calendar[monthIndex].days[dayIndex].isToday = true
      this.selectedMonth = this.calendar[monthIndex]
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
      }
    },
  }
</script>