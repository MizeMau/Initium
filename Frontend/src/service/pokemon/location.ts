import Service from '../service';
import type { PokemonLocation_Pokemon } from '@/service/pokemon/location_pokemon'

export interface PokemonLocation {
  pokemonLocationID: number
  created: Date
  deleted: Date | null
  name: string
}
export interface PokemonLocationFull extends PokemonLocation {
  encounter: Array<PokemonLocation_Pokemon>
  location: PokemonLocation
}

export interface PokemonLocation_Location {
  pokemonLocation_LocationID: number
  pokemonLocationID_From: number
  pokemonLocationID_To: number
  created: Date
  deleted: Date | null
  direction: number
  condition: string
}

export default class PokemonLocationService extends Service<PokemonLocation> {
  constructor() {
    super('/pokemon/location', 'pokemonLocationID')
  }

  getFullByLocation(pokemonLocationID: number) {
    return this.get<PokemonLocationFull>(`${pokemonLocationID}`, {
      pokemonSaveID: 1
    })
  }

  createLocationJunction(pokemonLocation_Location: PokemonLocation_Location | object) {
    return this.create('location', pokemonLocation_Location)
  }
}