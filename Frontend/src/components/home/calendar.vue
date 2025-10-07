<template>
  <div class="calendar-section" ref="calendar">
    
    <h2>
      <button class="search-btn"
              style="margin-right: auto"
              @click="subMonth">
        ⬅
      </button>
      {{months[selectedMonth]}} {{selectedYear}}
      <button class="search-btn"
              style="margin-left: auto"
              @click="addMonth">
        ➡
      </button>
    </h2>
    

    <div class="calendar">
      <!-- Weekday headers -->
      <div class="day" v-for="d in days" :key="d">{{ d }}</div>

      <!-- Dates -->
      <div class="date"
           :class="{'currentMonth': selectedMonth == i.date.getMonth()}"
           v-for="i in selectedDays"
           :key="i"
           @click="toggleDay(i, $event)">
        <div class="date-number">{{ i.date.getDate() }}</div>
        <span v-if="tasks[i]" class="task-dot"></span>
        <span v-else class="task-dot" style="opacity: 0"></span>
      </div>
    </div>

    <!-- Popover -->
    <div v-if="activeDay"
         class="task-popover"
         :style="popoverStyle">
      <h3>Day {{ activeDay }} 🗓️</h3>
      <ul>
        <li v-for="(task, idx) in tasks[activeDay]" :key="idx">🐾 {{ task }}</li>
      </ul>
      <button @click="closePopover" class="close-btn">Close</button>
    </div>
  </div>
</template>

<script>
  export default {
    name: "Calendar",
    data() {
      return {
        activeDay: null,
        popoverStyle: {},
        days: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
        months: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"],
        selectedMonth: 0,
        selectedYear: 0,
        selectedDays: [],
        tasks: {
          1: ["Fix bug 🐞", "Pet the cat 🐈"],
          5: ["Write documentation 📖", "Feed whiskers 🐾"],
          12: ["Code review 👨‍💻", "Nap with cat 😴🐱"],
          18: ["Push to GitHub 🚀", "Clean litter box 🧹🐈"],
          25: ["Learn new API 🔧", "Play with yarn 🧶"],
        },
      };
    },
    mounted() {
      const date = new Date();
      const month = date.getMonth()
      const year = date.getFullYear()
      this.changeCalenderDate(month, year)
      this.selectedMonth = month
      this.selectedYear = year
    },
    methods: {
      addMonth() {
        this.selectedMonth++
        if (this.selectedMonth > 11) {
          this.selectedMonth = 0
          this.selectedYear++
        }
        this.changeCalenderDate(this.selectedMonth, this.selectedYear)
      },
      subMonth() {
        this.selectedMonth--
        if (this.selectedMonth < 0) {
          this.selectedMonth = 11
          this.selectedYear--
        }
        this.changeCalenderDate(this.selectedMonth, this.selectedYear)
      },
      changeCalenderDate(month, year) {
        this.selectedDays = []
        this.activeDay = null;

        var firstDay = new Date(year, month, 1).getDay()
        firstDay = firstDay == 0 ? 6 : firstDay - 1
        for (let i = firstDay - 1; i >= 0; i--) {
          const date = new Date(year, month, -i)
          this.selectedDays.push({
            date
          })
        }

        var daysOfMonth = new Date(year, month + 1, 0).getDate()
        for (let i = 1; i <= daysOfMonth; i++) {
          const date = new Date(year, month, i)
          this.selectedDays.push({
            date
          })
        }

        var missingDates = 35 - this.selectedDays.length
        missingDates = missingDates < 0 ? 42 - this.selectedDays.length : missingDates
        for (let i = 1; i <= missingDates; i++) {
          const date = new Date(year, month + 1, i)
          this.selectedDays.push({
            date
          })
        }
      },
      toggleDay(day, event) {
        if (this.activeDay === day) {
          this.activeDay = null;
          return;
        }
        this.activeDay = day.date.toLocaleDateString()

        this.$nextTick(() => {
          const dateEl = event.currentTarget.getBoundingClientRect();
          const calendarEl = this.$refs.calendar.getBoundingClientRect();

          const popoverWidth = 300
          const margin = 8
          const popoverPadding = 16

          // Default to right side
          let left = dateEl.right - calendarEl.left + margin;
          let top = dateEl.top - calendarEl.top;

          // If not enough space to the right, place left
          if (left + popoverWidth > calendarEl.width) {
            left = dateEl.left - calendarEl.left - popoverWidth - margin - popoverPadding * 2
          }

          this.popoverStyle = {
            top: `${top}px`,
            left: `${left}px`,
            width: `${popoverWidth}px`,
            padding: `${popoverPadding}px`
          };
        });
      },
      closePopover() {
        this.activeDay = null;
      },
    },
  };
</script>

<style scoped>
  .search-btn {
    background: #3a3a3a;
    color: #fbc531;
    font-size: 1.2rem;
    border-radius: 50%;
    width: 40px;
    height: 40px;
    cursor: pointer;
  }

  .calendar-section {
    width: 80%;
    margin-top: 5rem;
    background: #1e1e1e;
    border-radius: 16px;
    padding: 2rem;
    box-shadow: 0 6px 16px rgba(0,0,0,0.5);
    position: relative;
  }

    .calendar-section h2 {
      text-align: center;
      margin-bottom: 1.5rem;
      color: #fbc531;
      font-weight: bold;
      display: flex;
      align-items: center;
      justify-content: center;
    }

  .calendar {
    display: grid;
    grid-template-columns: repeat(7, 1fr);
    gap: 0.7rem;
  }

  .day {
    font-weight: bold;
    text-align: center;
    color: #aaa;
    font-size: 0.9rem;
  }

  .currentMonth {
    background: #2b2b2b !important;
  }

  .date {
    text-align: center;
    padding: 1rem;
    border-radius: 10px;
    background: #232323;
    /*background: #2b2b2b;*/
    cursor: pointer;
    transition: all 0.2s ease;
    position: relative;
  }

    .date:hover {
      background: #3a3a3a;
      border: solid 1px #fbc531;
      transform: translateY(-5px) translateX(5px);
    }

  .date-number {
    font-weight: bold;
    font-size: 1.1rem;
    color: #f5f5f5;
  }

  .task-dot {
    width: 6px;
    height: 6px;
    background: #fbc531;
    border-radius: 50%;
    display: block;
    margin: 0.3rem auto 0;
  }

  /* Popover */
  .task-popover {
    position: absolute;
    background: #2b2b2b;
    border-radius: 12px;
    box-shadow: 0 6px 14px rgba(0,0,0,0.6);
    padding: 1rem;
    color: #eee;
    z-index: 100; /* stays above */
    animation: fadeIn 0.2s ease;
  }

    .task-popover h3 {
      margin-bottom: 0.5rem;
      color: #fbc531;
      font-size: 1rem;
    }

    .task-popover ul {
      list-style: none;
      padding: 0;
      margin: 0;
    }

    .task-popover li {
      margin: 0.4rem 0;
      font-size: 0.9rem;
    }

  .close-btn {
    margin-top: 0.6rem;
    background: #444;
    color: #eee;
    border: none;
    border-radius: 8px;
    padding: 0.4rem 0.8rem;
    cursor: pointer;
    transition: background 0.2s;
    font-size: 0.85rem;
  }

    .close-btn:hover {
      background: #555;
    }

  @keyframes fadeIn {
    from {
      opacity: 0;
    }

    to {
      opacity: 1;
    }
  }
</style>
