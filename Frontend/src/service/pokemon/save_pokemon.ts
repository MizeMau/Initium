import Service from '../service';

export interface PokemonSave_Pokemon {
  pokemonSave_PokemonID: number
  pokemonSaveID: number
  pokemonPokemonID: number
  created: Date
  deleted: Date | null
}

export default class PokemonSave_PokemonService extends Service<PokemonSave_Pokemon> {
  constructor() {
    super('/pokemon/save/pokemon', 'pokemonSave_PokemonID')
  }

  async deleteEntry(pokemonPokemonID: number, pokemonSaveID: number) {
    const response = await this.api.delete<boolean>(`?pokemonPokemonID=${pokemonPokemonID}&pokemonSaveID=${pokemonSaveID}`)
    return response.data
  }
}