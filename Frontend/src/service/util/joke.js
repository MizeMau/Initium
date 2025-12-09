import axios from 'axios';

export default class JokeService {
    constructor() {
        this.api = axios.create({
            baseURL: 'https://v2.jokeapi.dev/joke',
            headers: {
                "Content-Type": 'application/json',
            },
            withCredentials: false,
        });
    }

    async getAny(amount) {
        const response = await this.api.get('Any', { params: { amount } });
        return response.data;
    }
}
