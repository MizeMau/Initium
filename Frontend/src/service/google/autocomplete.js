// Autocomplete.js
import Service from '../service.js';

export default class Autocomplete extends Service {
    constructor() {
        super('/google/autocomplete'); // path for this API
    }

    getAutocompleteByQuery(query) {
        return this.getAll('', { query: query });
    }
}
