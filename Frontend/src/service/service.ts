import axios, { AxiosInstance, AxiosError } from 'axios'
import router from '@/router'

export default class Service<TDefault> {
    protected api: AxiosInstance
    constructor(path: string) {
        this.api = axios.create({
            baseURL: `http://localhost:5045${path}`,
            headers: {
                'Content-Type': 'application/json',
            },
            withCredentials: true,
        })
        this.api.interceptors.response.use(
            response => response,
            (error: AxiosError) => {
                if (error.response?.status === 401) {
                    router.push('/login')
                }
                return Promise.reject(error)
            }
        )
    }

    async get<T = TDefault>(
        endpoint = '',
        params?: Record<string, unknown>
    ): Promise<T> {
        const response = await this.api.get<T>(endpoint, { params })
        return response.data
    }

    async getAll<T = TDefault>(
        endpoint = '',
        params?: Record<string, unknown>
    ): Promise<T[]> {
        const response = await this.api.get<T[]>(endpoint, { params })
        return response.data
    }

    async getById<T = TDefault>(
        endpoint = '',
        id: string | number
    ): Promise<T> {
        const response = await this.api.get<T>(`${endpoint}/${id}`)
        return response.data
    }

    async create<T = TDefault>(
        endpoint = '',
        body: T
    ): Promise<T> {
        const response = await this.api.post<T>(endpoint, body)
        return response.data
    }

    async update<T = TDefault>(
        endpoint = '',
        body: T
    ): Promise<T> {
        const response = await this.api.put<T>(`${endpoint}`, body)
        return response.data
    }

    async delete<T = TDefault>(
        endpoint = '',
        id: string | number
    ): Promise<T> {
        const response = await this.api.delete<T>(`${endpoint}/${id}`)
        return response.data
    }
}
