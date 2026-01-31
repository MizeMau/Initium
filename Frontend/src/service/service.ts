import axios, { AxiosInstance, AxiosError } from 'axios'
import router from '@/router'

export default class Service<TDefault> {
    protected api: AxiosInstance
    protected key: string
    constructor(path: string, key: string) {
        this.key = key
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
        const response = await this.getAll<T>(endpoint, params)
        if (!Array.isArray(response))
            return response
        if (response.length === 0)
            return null as T
        return response[0]
    }

    async getAll<T = TDefault[]>(
        endpoint = '',
        params?: Record<string, unknown>
    ): Promise<T> {
        const response = await this.api.get<T>(endpoint, { params })
        return response.data
    }

    async getById<T = TDefault>(
        id: string | number
    ): Promise<T> {
        if (this.key == null) {
            console.error('No key defined')
        }
        const response = await this.get<T>('', {
            [this.key]: id
        })
        return response
    }

    async create<T = TDefault>(
        body: T
    ): Promise<T>
    async create<T = TDefault>(
        endpoint: string,
        body: T
    ): Promise<T>
    async create<T = TDefault>(
        endpointOrBody: string | T,
        maybeBody?: T
    ): Promise<T> {
        const endpoint =
            typeof endpointOrBody === 'string'
                ? endpointOrBody
                : ''

        const body =
            typeof endpointOrBody === 'string'
                ? maybeBody!
                : endpointOrBody

        const response = await this.api.post<T>(endpoint, body)
        return response.data
    }


    async update<T = TDefault>(
        body: T
    ): Promise<T>
    async update<T = TDefault>(
        endpoint: string,
        body: T
    ): Promise<T>
    async update<T = TDefault>(
        endpointOrBody: string | T,
        maybeBody?: T
    ): Promise<T> {
        const endpoint =
            typeof endpointOrBody === 'string'
                ? endpointOrBody
                : ''

        const body =
            typeof endpointOrBody === 'string'
                ? maybeBody!
                : endpointOrBody

        const response = await this.api.put<T>(endpoint, body)
        return response.data
    }


    async delete(
        endpoint = '',
        id: string | number
    ): Promise<boolean> {
        const response = await this.api.delete<boolean>(`${endpoint}?id=${id}`)
        return response.data
    }
}
