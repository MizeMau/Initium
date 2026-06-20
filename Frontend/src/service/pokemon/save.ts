import Service from '../service';

export interface PokemonSave {
  pokemonSaveID: number
  created: Date
  deleted: Date | null
  name: string
}

export interface PokemonSave_Pokemon {
  pokemonSave_PokemonID: number
  pokemonSaveID: number
  pokemonPokemonID: number
  created: Date
  deleted: Date | null
}

export default class PokemonSaveService extends Service<PokemonSave> {
  constructor() {
    super('/pokemon/save', 'pokemonSaveID')
  }

  async createPokemonJunction(pokemonSave_Pokemon: PokemonSave_Pokemon | object) {
    return this.create('pokemon', pokemonSave_Pokemon)
  }

  async deletePokemonJunction(pokemonPokemonID: number, pokemonSaveID: number) {
    const response = await this.api.delete<boolean>(`pokemon?pokemonPokemonID=${pokemonPokemonID}&pokemonSaveID=${pokemonSaveID}`)
    return response.data
  }
}