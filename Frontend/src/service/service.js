// Service.js
import axios from 'axios';
import router from '@/router';

export default class Service {
    constructor(path) {
        this.api = axios.create({
            baseURL: 'http://localhost:5045' + path,
            headers: {
                "Content-Type": 'application/json',
            },
            withCredentials: true,
        });
        this.api.interceptors.response.use(
            response => response,
            error => {
                if (error.response && error.response.status === 401) {
                    // e.g. redirect to login route in your Vue app
                    router.push('/login');
                }
                return Promise.reject(error);
            }
        );
    }

    async getAll(endpoint, params = {}) {
        const response = await this.api.get(endpoint, { params });
        return response.data;
    }

    async getById(endpoint, id) {
        const response = await this.api.get(`${endpoint}/${id}`);
        return response.data;
    }

    async create(endpoint, body) {
        const response = await this.api.post(endpoint, body);
        return response.data;
    }

    async update(endpoint, id, body) {
        const response = await this.api.put(`${endpoint}/${id}`, body);
        return response.data;
    }

    async delete(endpoint, id) {
        const response = await this.api.delete(`${endpoint}/${id}`);
        return response.data;
    }
}
