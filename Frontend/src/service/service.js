// Service.js
import axios from 'axios';

export default class Service {
    constructor(path) {
        this.api = axios.create({
            baseURL: 'http://localhost:5045' + path,
            headers: {
                "Content-Type": 'application/json'
            }
        });
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
