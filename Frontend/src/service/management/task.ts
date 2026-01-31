import Service from '../service';

export interface ManagementTask {
    managementTaskID: number
    created: Date
    deleted: Date | null
    name: string
    description: string
    isCompleted: boolean
    isMilestone: boolean
    start: Date | null
    end: Date | null
    managementProjectID: number
    managementSectionID: number
}
export interface ManagementTaskFull extends ManagementTask {
    tasks: Array<ManagementTask>
}
export default class ManagementTaskService extends Service<ManagementTask> {
    constructor() {
        super('/management/task', 'managementTaskID')
    }
}