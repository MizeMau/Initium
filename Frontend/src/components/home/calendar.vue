<template>
  <div class="calendar-section" ref="calendar">
    <h2>October 2025</h2>

    <div class="calendar">
      <!-- Weekday headers -->
      <div class="day" v-for="d in days" :key="d">{{ d }}</div>

      <!-- Dates -->
      <div class="date"
           v-for="i in 31"
           :key="i"
           @click="toggleDay(i, $event)">
        <div class="date-number">{{ i }}</div>
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
        days: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
        tasks: {
          1: ["Fix bug 🐞", "Pet the cat 🐈"],
          5: ["Write documentation 📖", "Feed whiskers 🐾"],
          12: ["Code review 👨‍💻", "Nap with cat 😴🐱"],
          18: ["Push to GitHub 🚀", "Clean litter box 🧹🐈"],
          25: ["Learn new API 🔧", "Play with yarn 🧶"],
        },
      };
    },
    methods: {
      toggleDay(day, event) {
        if (this.activeDay === day) {
          this.activeDay = null;
          return;
        }
        this.activeDay = day;

        this.$nextTick(() => {
          const dateEl = event.currentTarget.getBoundingClientRect();
          const calendarEl = this.$refs.calendar.getBoundingClientRect();

          const popoverWidth = 300;
          const margin = 8;
          const popoverPadding = 16;

          // Default to right side
          let left = dateEl.right - calendarEl.left + margin;
          let top = dateEl.top - calendarEl.top;

          // If not enough space to the right, place left
          if (left + popoverWidth > calendarEl.width) {
            left = dateEl.left - calendarEl.left - popoverWidth - margin - popoverPadding * 2;
            console.log('dateEl.left', dateEl.left);
            console.log('calendarEl.left', calendarEl.left);
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

  .date {
    text-align: center;
    padding: 1rem;
    border-radius: 10px;
    background: #2b2b2b;
    cursor: pointer;
    transition: all 0.2s ease;
    position: relative;
  }

    .date:hover {
      background: #3a3a3a;
      transform: translateY(-2px);
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
