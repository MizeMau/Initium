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

export default class PokemonLocationService extends Service<PokemonLocation> {
  constructor() {
    super('/pokemon/location', 'pokemonLocationID')
  }

  getFullByLocation(pokemonLocationID: number) {
    return this.get<PokemonLocationFull>(`${pokemonLocationID}`, {
      pokemonSaveID: 1
    })
  }
}