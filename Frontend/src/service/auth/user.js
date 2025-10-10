// Autocomplete.js
import Service from '../service.js';

export default class User extends Service {
    constructor() {
        super('/auth/user');
    }

    login(username, password) {
        return this.getAll('login', { username: username, password: password });
    }

    logout() {
        return this.getAll('/logout');
    }

    getCurrentUser() {
        return this.getAll('/currentUser');
    }
}
