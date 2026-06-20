<template>
  <div class="container mt-3">
    <div v-if="!location">
      <div class="spinner-border" />
    </div>
    <div v-else>
      <h2>
        {{location.name}}
      </h2>
      <div class="row">
        <div class="col-md-8">
          <table class="table table-bordered">
            <thead>
              <tr>
                <th rowspan="2"></th>
                <th rowspan="2">Pokémon</th>
                <th rowspan="2" colspan="2">Type</th>
                <th rowspan="2">Level</th>
                <th rowspan="2">Catch Rate</th>
                <th colspan="3">Encounter<br />Rate</th>
              </tr>
              <tr>
                <th>Dawn</th>
                <th>Noon</th>
                <th>Dusk</th>
              </tr>
            </thead>
            <tbody>
              <template v-for="key in Object.keys(encounters)">
                <tr>
                  <th colspan="10"
                      class="text-center"
                      v-bind:style="{'background-color': PokemonEncounterService.getTypeHex(key)}">
                    {{key}}
                  </th>
                </tr>
                <tr v-for="encounter in encounters[key]">
                  <th class="p-0">
                    <img v-bind:src="`https://ifd-spaces.sfo2.cdn.digitaloceanspaces.com/custom/${encounter.pokemonPokemonID}.png`"
                         height="64" />
                  </th>
                  <th v-bind:class="{'bg-success': encounter.isCaught}">
                    <button class="btn"
                            @click="caughtPokemon(encounter)">
                      {{encounter.pokemonName}}
                    </button>
                  </th>
                  <th v-bind:colspan="encounter.typeName_Second == null ? 2 : 1"
                      v-bind:style="{'background-color': PokemonTypeService.getTypeHex(encounter.typeName_First)}">
                    {{encounter.typeName_First}}
                  </th>
                  <th v-if="encounter.typeName_Second"
                      v-bind:style="{'background-color': PokemonTypeService.getTypeHex(encounter.typeName_Second)}">
                    {{encounter.typeName_Second}}
                  </th>
                  <th>{{encounter.level_Low}} - {{encounter.level_High}}</th>
                  <th>{{encounter.catchRate}}</th>
                  <th v-if="encounter.dawn">
                    <router-link v-if="encounter.avgEncounterPercentage < encounter.avgEncounterPercentage_Best"
                                 v-bind:to="`/pokemon/location/${encounter.pokemonLocationID_Best}`"
                                 class="text-body"
                                 v-bind:title="`The best location is ${encounter.pokemonLocationName_Best} (${encounter.avgEncounterPercentage_Best} > ${encounter.avgEncounterPercentage})`">
                      {{encounter.dawn}}%
                    </router-link>
                    <span v-else>
                      {{encounter.dawn}}%
                    </span>
                  </th>
                  <th v-else>
                    <router-link v-if="encounter.avgEncounterPercentage < encounter.avgEncounterPercentage_Best"
                                 v-bind:to="`/pokemon/location/${encounter.pokemonLocationID_Best}`"
                                 class="text-body"
                                 v-bind:title="`The best location is ${encounter.pokemonLocationName_Best} (${encounter.avgEncounterPercentage_Best} > ${encounter.avgEncounterPercentage})`">
                      -
                    </router-link>
                    <span v-else>
                      -
                    </span>
                  </th>
                  <th v-if="encounter.noon">
                    <router-link v-if="encounter.avgEncounterPercentage < encounter.avgEncounterPercentage_Best"
                                 v-bind:to="`/pokemon/location/${encounter.pokemonLocationID_Best}`"
                                 class="text-body"
                                 v-bind:title="`The best location is ${encounter.pokemonLocationName_Best} (${encounter.avgEncounterPercentage_Best} > ${encounter.avgEncounterPercentage})`">
                      {{encounter.noon}}%
                    </router-link>
                    <span v-else>
                      {{encounter.noon}}%
                    </span>
                  </th>
                  <th v-else>
                    <router-link v-if="encounter.avgEncounterPercentage < encounter.avgEncounterPercentage_Best"
                                 v-bind:to="`/pokemon/location/${encounter.pokemonLocationID_Best}`"
                                 class="text-body"
                                 v-bind:title="`The best location is ${encounter.pokemonLocationName_Best} (${encounter.avgEncounterPercentage_Best} > ${encounter.avgEncounterPercentage})`">
                      -
                    </router-link>
                    <span v-else>
                      -
                    </span>
                  </th>
                  <th v-if="encounter.dusk">
                    <router-link v-if="encounter.avgEncounterPercentage < encounter.avgEncounterPercentage_Best"
                                 v-bind:to="`/pokemon/location/${encounter.pokemonLocationID_Best}`"
                                 class="text-body"
                                 v-bind:title="`The best location is ${encounter.pokemonLocationName_Best} (${encounter.avgEncounterPercentage_Best} > ${encounter.avgEncounterPercentage})`">
                      {{encounter.dusk}}%
                    </router-link>
                    <span v-else>
                      {{encounter.dusk}}%
                    </span>
                  </th>
                  <th v-else>
                    <router-link v-if="encounter.avgEncounterPercentage < encounter.avgEncounterPercentage_Best"
                                 v-bind:to="`/pokemon/location/${encounter.pokemonLocationID_Best}`"
                                 class="text-body"
                                 v-bind:title="`The best location is ${encounter.pokemonLocationName_Best} (${encounter.avgEncounterPercentage_Best} > ${encounter.avgEncounterPercentage})`">
                      -
                    </router-link>
                    <span v-else>
                      -
                    </span>
                  </th>
                </tr>
              </template>
            </tbody>
          </table>
        </div>
        <div class="col-md-4">
          <table class="table table-bordered">
            <tbody>
              <tr>
                <td colspan="2"
                    class="p-0 rounded-top-1">
                  <img class="img-fluid rounded-top-1"
                       v-bind:src="`http://${url}:5045/uploads/pokemon/location/${location.pokemonLocationID}.webp`"
                       v-bind:alt="`todo Image - ${location.pokemonLocationID}`" />
                </td>
              </tr>
              <tr v-for="pokemonRoute in location.routes">
                <td>
                  <i class="bi"
                     v-bind:class="PokemonRouteService.getDirectionIcon(pokemonRoute.direction)" />
                </td>
                <td>
                  <router-link v-bind:to="`/pokemon/location/${pokemonRoute.pokemonLocationID_To}`"
                               class="nav-link">
                    {{pokemonRoute.name}}
                  </router-link>
                  <small>
                    {{pokemonRoute.condition}}
                  </small>
                </td>
              </tr>
              <tr>
                <td colspan="2">
                  <button type="button"
                          class="btn btn-primary container-fluid"
                          data-bs-toggle="modal"
                          data-bs-target="#locationJunctionModal">
                    Edit connected connections
                  </button>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
      <LocationJunctionModal v-bind:location="location"
                             @updateLocation="getData"/>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, watch } from 'vue'
  import { useRoute } from 'vue-router'
  import PokemonLocationService from '@/service/pokemon/location'
  import type { PokemonLocationFull } from '@/service/pokemon/location'
  import PokemonEncounterService from '@/service/pokemon/encounter'
  import type { PokemonEncounter } from '@/service/pokemon/encounter'
  import PokemonTypeService from '@/service/pokemon/type'
  import PokemonSaveService from '@/service/pokemon/save'
  import type { PokemonSave_Pokemon } from '@/service/pokemon/save'
  import PokemonRouteService from '@/service/pokemon/route'

  import LocationJunctionModal from './location-junction-modal.vue'

  const route = useRoute()
  const url = window.location.hostname

  const pokemonLocationService = new PokemonLocationService()
  const pokemonSaveService = new PokemonSaveService()

  const location = ref<PokemonLocationFull>()
  const encounters = ref<object>()

  onMounted(async () => {
    await getData()
  })

  watch(() => route.params.id, async () => {
    await getData()
  })

  async function getData() {
    const pokemonLocationID: number = +route.params.id
    if (pokemonLocationID == null) return
    const tmp = await pokemonLocationService.getFullByLocation(pokemonLocationID)

    encounters.value = Object.groupBy(tmp.encounter, g => PokemonEncounterService.getTypeName(g.type))
    location.value = tmp
  }

  async function caughtPokemon(encounter: PokemonEncounter) {
    var success;
    if (!encounter.isCaught) {
      success = await pokemonSaveService.createPokemonJunction({
        pokemonPokemonID: encounter.pokemonPokemonID,
        pokemonSaveID: 1,
      }) 
    }
    else {
      var sure = confirm("Did you set him free?")
      if (!sure)
        return;
      success = await pokemonSaveService.deletePokemonJunction(encounter.pokemonPokemonID, 1)
    }
    if (success) {
      for (let key of Object.keys(encounters.value)) {
        for (let encounterItem of encounters.value[key] as PokemonEncounter[]) {
          if (encounterItem.pokemonPokemonID != encounter.pokemonPokemonID)
            continue
          encounterItem.isCaught = !encounterItem.isCaught
        }
      }
    }
  }
</script>