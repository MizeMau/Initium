import Service from '../service';

export interface ManagementTask {
    managementTaskID: number
    created: Date
    deleted: Date
    name: string
    managementProjectID: number
    managementSectionID: number
}

export default class ManagementTaskService extends Service<ManagementTask> {
    constructor() {
        super('/management/task', 'managementTaskID')
    }
}