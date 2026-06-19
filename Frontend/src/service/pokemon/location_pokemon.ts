import Service from '../service';

export interface PokemonLocation_Pokemon {
  pokemonLocation_PokemonID: number
  pokemonPokemonID: number
  pokemonLocationID: number
  pokemonModeID: number
  pokemonName: string
  typeName_First: string
  typeName_Second: string | null
  level_Low: number
  level_High: number
  catchRate: number
  dawn: number | null
  noon: number | null
  dusk: number | null
  type: number
  avgEncounterPercentage: number | null
  pokemonLocationID_Best: number
  pokemonLocationName_Best: string
  bestAVGEncounterPercentage: number | null
  isCaught: boolean
}

export default class PokemonLocation_PokemonService extends Service<PokemonLocation_Pokemon> {
  constructor() {
    super('/pokemon/encounter', 'pokemonLocation_PokemonID')
  }

  static getTypeName(type: number) {
    switch (type) {
      case 1:
        return "Grass"
      case 2:
        return "PokeRadar"
      case 3:
        return "Surf"
      case 4:
        return "OldRod"
      case 5:
        return "GoodRod"
      case 6:
        return "SuperRod"
      case 7:
        return "RockSmash"
      case 8:
        return "Cave"
      default:
        return "Error"
    }
  }

  static getTypeHex(type: string) {
    switch (type) {
      case "Grass":
        return "#94DB84"
      case "PokeRadar":
        return "#A058E8"
      case "Surf":
        return "#2888E0"
      case "OldRod":
      case "GoodRod":
      case "SuperRod":
        return "#1060B8"
      case "RockSmash":
        return "#B39661"
      case "Cave":
        return "#948C84"
      default:
        return "#000000"
    }
  }
}