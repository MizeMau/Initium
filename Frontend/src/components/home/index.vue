<template>
  <div class="container">
    <div class="row mt-5">
      <div class="col-md-12">
        <div class="input-group mb-3">
          <input ref="googleSearchBar"
                 type="text" 
                 class="form-control"
                 placeholder="Search Google..."
                 v-model="query"
                 v-on:input="googleSearchInput">
          <button class="btn btn-primary">🐱</button>
        </div>
      </div>
    </div>
    <div class="row">
      <div>

      </div>
    </div>
    <!-- Search Section -->
    <!--<section class="search-section">
      <div class="search-box">
        <input ref="googleSearchBar"
               type="text"
               placeholder="Search Google..."
               v-model="query"
               @input="googleSearchInput" />
        <button class="search-btn"
                @click="suggestionClick(query)">🐱</button>
      </div>-->

      <!-- Suggestions (Mockup) -->
      <!--<ul v-if="suggestions.length > 0" class="suggestions">
        <li v-for="(suggestion, index) in suggestions"
            :key="`suggestion_${index}`"
            @click="suggestionClick(suggestion)"
            :class="{'suggestions-li-selected': selectedSuggestion == index}">
          <b v-if="suggestion.hasQuery">{{tmpQuery}}</b>{{suggestion.query}}
        </li>
      </ul>
    </section>-->
    <!--<calender-component />-->
  <div class="mt-5">
    <ul v-if="suggestions.length > 0">
      <li v-for="(suggestion, index) in suggestions"
          :key="`suggestion_${index}`"
          @click="suggestionClick(suggestion)">
        <b v-if="suggestion.hasQuery">{{tmpQuery}}</b>{{suggestion.query}}
      </li>
    </ul>
  </div>
  </div>
</template>

<script>
  //import CalenderComponent from './calendar.vue'

  import AutocompleteService from "@/service/google/autocomplete"

  import { useDebounceFn } from "@vueuse/core"

  export default {
    name: "HomePage",
    //components: {
    //  'calender-component': CalenderComponent
    //},
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
        autocompleteService: new AutocompleteService(),
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