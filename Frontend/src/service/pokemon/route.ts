import Service from '../service';

export interface PokemonRoute {
  pokemonLocation_LocationID: number
  pokemonLocationID_From: number
  pokemonLocationID_To: number
  name: string
  direction: number
  condition: string
}

export default class PokemonRouteService extends Service<PokemonRoute> {
  constructor() {
    super('/pokemon/route', 'pokemonRouteID')
  }

  static getDirectionName(direction: number) {
    switch (direction) {
      case 1:
        return "Up"
      case 2:
        return "UpRight"
      case 3:
        return "Right"
      case 4:
        return "DownRight"
      case 5:
        return "Down"
      case 6:
        return "DownLeft"
      case 7:
        return "Left"
      case 8:
        return "UpLeft"
      default:
        return ""
    }
  }

  static getDirectionIcon(direction: number) {
    switch (direction) {
      case 1:
        return "bi-arrow-up"
      case 2:
        return "bi-arrow-up-right"
      case 3:
        return "bi-arrow-right"
      case 4:
        return "bi-arrow-down-right"
      case 5:
        return "bi-arrow-down"
      case 6:
        return "bi-arrow-down-left"
      case 7:
        return "bi-arrow-left"
      case 8:
        return "bi-arrow-up-left"
      default:
        return ""
    }
  }
}