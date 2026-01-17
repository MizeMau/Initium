import Service from '../service'
export default class GoogleAutocompleteService extends Service<string> {
    constructor() {
        super('/google/autocomplete')
    }

    getAutocompleteByQuery(query: string) {
        return this.getAll('', {
            query: query
        })
    }
}
