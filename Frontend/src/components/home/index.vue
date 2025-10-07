<template>
  <div class="homepage">
    <!-- Search Section -->
    <section class="search-section">
      <div class="search-box">
        <input ref="googleSearchBar"
               type="text"
               placeholder="Search Google..."
               v-model="query"
               @input="googleSearchInput" />
        <button class="search-btn"
                @click="suggestionClick(query)">🐱</button>
      </div>

      <!-- Suggestions (Mockup) -->
      <ul v-if="suggestions.length > 0" class="suggestions">
        <li v-for="(suggestion, index) in suggestions"
            :key="`suggestion_${index}`"
            @click="suggestionClick(suggestion)"
            :class="{'suggestions-li-selected': selectedSuggestion == index}">
          <b v-if="suggestion.hasQuery">{{tmpQuery}}</b>{{suggestion.query}}
        </li>
      </ul>
    </section>

    <calender-component />
  </div>
</template>

<script>
  import CalenderComponent from './calendar.vue'

  import Autocomplete from "@/service/google/autocomplete"

  import { useDebounceFn } from "@vueuse/core"

  export default {
    name: "HomePage",
    components: {
      'calender-component': CalenderComponent
    },
    mounted() {
      this.runAutocomplete = useDebounceFn(async (uriQuery) => {
        if (!uriQuery) {
          this.suggestions = []
          return
        }
        const data = await this.autocompleteService.getAutocompleteByQuery(uriQuery)
        this.tmpQuery = this.query.trim()
        this.selectedSuggestion = null
        this.suggestions = data.map(m => {
          console.log('suggestion', m)
          return {
            hasQuery: m.includes(this.tmpQuery),
            query: m.replace(this.tmpQuery, ''),
          }
        })
      }, 300)

      window.addEventListener('keydown', this.handleKeydown);

      this.$refs.googleSearchBar.focus();
    },
    beforeDestroy() {
      window.removeEventListener('keydown', this.handleKeydown);
    },
    data() {
      return {
        query: "",
        tmpQuery: "",
        suggestions: [],
        selectedSuggestion: null,
        autocompleteService: new Autocomplete(),
      }
    },
    methods: {
      async googleSearchInput() {
        const uriQuery = encodeURIComponent(this.query)
        this.runAutocomplete(uriQuery)
      },
      suggestionClick(query) {
        if(!query) return
        const uriQuery = encodeURIComponent(query)
        window.open(`https://www.google.com/search?q=${uriQuery}`, '_self')
      },
      async handleKeydown(e) {
        const key = e.key
        if (key == 'ArrowUp') {
          if (this.selectedSuggestion <= 0) return
          this.selectedSuggestion--
          const suggestion = this.suggestions[this.selectedSuggestion]
          this.query = suggestion.hasQuery ? this.tmpQuery + suggestion.query : suggestion.query
        }
        if (key == 'ArrowDown') {
          if (this.selectedSuggestion == null) this.selectedSuggestion = -1
          if (this.selectedSuggestion >= this.suggestions.length - 1) return
          this.selectedSuggestion++
          const suggestion = this.suggestions[this.selectedSuggestion]
          this.query = suggestion.hasQuery ? this.tmpQuery + suggestion.query : suggestion.query
        }
        if (key == "Enter") {
          this.suggestionClick(this.query)
        }
      }
    },
  }
</script>

<style scoped>
  /* General Layout */
  .homepage {
    color: #ddd;
    background: linear-gradient(135deg, #1a1a1a, #121212);
    min-height: 100vh;
    display: flex;
    flex-direction: column;
    align-items: center;
    font-family: "Fira Code", monospace;
  }

  .search-box {
    display: flex;
    align-items: center;
    background: #2c2c2c;
    border-radius: 50px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.3);
    padding: 0.5rem 1rem;
  }

    .search-box input {
      flex: 1;
      border: none;
      outline: none;
      padding: 0.7rem;
      font-size: 1rem;
      border-radius: 50px;
      background: transparent;
      color: #eee;
    }

  .search-btn {
    background: #3a3a3a;
    border: none;
    color: #fbc531;
    font-size: 1.2rem;
    border-radius: 50%;
    width: 40px;
    height: 40px;
    cursor: pointer;
  }

  /* Search Section */
  .search-section {
    width: 100%;
    max-width: 600px;
    margin-bottom: 2rem;
    position: relative; /* <- important */
    z-index: 5; /* keep it above calendar */
    margin-top: 5rem;
  }

  .suggestions {
    position: absolute; /* float over content */
    top: 100%; /* right below search box */
    left: 0;
    right: 0;
    background: #2b2b2b;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0,0,0,0.4);
    list-style: none;
    padding: 0.5rem 0;
    overflow-y: auto;
    z-index: 10; /* ensures overlap */
  }

    .suggestions li {
      padding: 0.7rem 1rem;
      cursor: pointer;
      color: #ccc;
    }

      .suggestions li:hover {
        background: #3c3c3c;
        color: #fff;
      }
      .suggestions-li-selected {
        background: #3c3c3c;
        color: #fff;
      }
</style>
