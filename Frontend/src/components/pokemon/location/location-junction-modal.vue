<template>
  <div class="modal fade" id="locationJunctionModal" tabindex="-1" aria-labelledby="locationJunctionModal" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="locationJunctionModal">Connect to {{location.name}} - {{location.pokemonLocationID}}</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <div class="row">
            <div class="col-3">
              <select v-model="direction"
                      class="form-select"
                      v-bind:class="{'is-invalid': error.direction}">
                <option selected disabled v-bind:value="null">Direction</option>
                <option value="1">↑</option>
                <option value="2">↗</option>
                <option value="3">→</option>
                <option value="4">↘</option>
                <option value="5">↓</option>
                <option value="6">↙</option>
                <option value="7">←</option>
                <option value="8">↖</option>
              </select>
              <div class="invalid-feedback">
                {{error.direction}}
              </div>
            </div>
            <div class="col-9">
              <select v-model="locationID_To"
                      class="form-select"
                      v-bind:class="{'is-invalid': error.pokemonLocationID_To}">
                <option selected disabled v-bind:value="null">Location</option>
                <option v-for="junctionLocation in locations"
                        v-bind:value="junctionLocation.pokemonLocationID">
                  {{junctionLocation.name}} - {{junctionLocation.pokemonLocationID}}
                </option>
              </select>
              <div class="invalid-feedback">
                {{error.pokemonLocationID_To}}
              </div>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col-12">
              <input v-model="condition"
                     type="text"
                     placeholder="Condition"
                     class="form-control"
                     v-bind:class="{'is-invalid': error.condition}"
                     maxlength="68" />
              <div class="invalid-feedback">
                {{error.condition}}
              </div>
            </div>
          </div>
          <div class="row mt-3">
            <div class="col-12">
              <button class="btn btn-primary float-end"
                      @click="add">
                Add
              </button>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
        </div>
      </div>
    </div>
  </div>
</template>


<script setup lang="ts">
  import { ref, onMounted } from 'vue'
  import PokemonLocationService from '@/service/pokemon/location'
  import type { PokemonLocation, PokemonLocationFull } from '@/service/pokemon/location'

  const emit = defineEmits(['updateLocation'])

  const props = defineProps<{
    location: PokemonLocationFull
  }>()

  const pokemonLocationService = new PokemonLocationService()

  const locations = ref<PokemonLocation[]>()
  const locationID_To = ref<Number | null>(null)
  const direction = ref<Number | null>(null)
  const condition = ref<string | null>(null)
  const error = ref<object>({})

  onMounted(async () => {
    await getData()
  })

  async function getData() {
    const locationData = await pokemonLocationService.getAll()
    locations.value = locationData.sort((a, b) => a.name.localeCompare(b.name))
  }

  async function add() {
    const response = await pokemonLocationService.createLocationJunction({
      pokemonLocationID_From: props.location.pokemonLocationID,
      pokemonLocationID_To: locationID_To.value,
      direction: direction.value,
      condition: condition.value,
    })
    if (response != null && !response._containsError) {
      error.value = {}
      emit('updateLocation')
    }
    else {
      error.value = response
    }
  }
</script>