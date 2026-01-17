import Service from '../service';

export interface User {
    backendUserID: number
    created: Date
    deleted: Date
    username: string
    password: string | undefined
}

export default class UserService extends Service<User> {
    constructor() {
        super('/auth/user')
    }

    login(username: string, password: string) {
        return this.get('login', {
            username: username,
            password: password,
        });
    }

    logout() {
        return this.get('/logout')
    }

    getCurrentUser() {
        return this.get('/currentUser')
    }
}
