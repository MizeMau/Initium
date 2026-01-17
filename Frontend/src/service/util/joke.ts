import axios, { AxiosInstance } from 'axios';

export default class JokeService {
    protected api: AxiosInstance
    constructor() {
        this.api = axios.create({
            baseURL: 'https://v2.jokeapi.dev/joke',
            headers: {
                "Content-Type": 'application/json',
            },
            withCredentials: false,
        });
    }

    async getAny(amount: number) {
        const response = await this.api.get('Any', { params: { amount } });
        return response.data;
    }
}
