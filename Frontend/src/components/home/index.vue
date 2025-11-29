<template>
  <div class="container">
    <div class="row mt-5">
      <div class="col-md-12 position-relative">
        <div class="input-group">
          <div class="form-floating">
            <input ref="googleSearchBar"
                   type="text"
                   class="form-control"
                   placeholder="Search Google..."
                   v-model="query"
                   v-on:input="googleSearchInput"
                   v-on:focus="onFocusChange(true)" 
                   v-on:blur="onFocusChange(false)"/>
            <label for="floatingInput">Search Google...</label>
          </div>
          <button class="btn btn-primary"
                  v-on:click="suggestionClick(query)">
            🐱
          </button>
        </div>
        <ul ref="suggestionsDropdown"
            class="dropdown-menu w-100">
          <li v-for="(suggestion, index) in suggestions"
              v-bind:key="`suggestion_${index}`"
              v-on:click="suggestionClick(suggestion)"
              class="dropdown-item"
              v-bind:class="{ active: selectedSuggestion === index }">
            {{suggestion}}
          </li>
        </ul>
      </div>
    </div>
    <calender-component />
  </div>
</template>

<script>
  import CalenderComponent from './calendar.vue'

  import AutocompleteService from "@/service/google/autocomplete"

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
          this.$refs.suggestionsDropdown.classList.remove('show')
          return
        }
        const data = await this.autocompleteService.getAutocompleteByQuery(uriQuery)
        this.selectedSuggestion = null
        this.suggestions = data

        if (this.suggestions.length > 0 && this.isInputFocused) {
          this.$refs.suggestionsDropdown.classList.add('show')
        } else {
          this.$refs.suggestionsDropdown.classList.remove('show')
        }

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
        suggestions: [],
        selectedSuggestion: null,
        isInputFocused: false,
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
          this.query = this.suggestions[this.selectedSuggestion]
        }
        if (key == 'ArrowDown') {
          if (this.selectedSuggestion == null) this.selectedSuggestion = -1
          if (this.selectedSuggestion >= this.suggestions.length - 1) return
          this.selectedSuggestion++
          this.query = this.suggestions[this.selectedSuggestion]
        }
        if (key == "Enter") {
          this.suggestionClick(this.query)
        }
      },
      onFocusChange(isInputFocused) {
        this.isInputFocused = isInputFocused
        if (!isInputFocused) {
          setTimeout(() => {
            this.$refs.suggestionsDropdown.classList.remove("show")
          }, 50)
        }
        else if (this.suggestions.length) {
          this.$refs.suggestionsDropdown.classList.add("show")
        }
      }
    },
  }
</script>