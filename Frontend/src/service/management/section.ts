import Service from '../service';
import type { ManagementTaskFull } from '@/service/management/task'

export interface ManagementSection {
    managementSectionID: number
    created: Date
    deleted: Date | null
    name: string
    managementProjectID: number
}
export interface ManagementSectionFull extends ManagementSection {
    tasks: Array<ManagementTaskFull>
}
export default class ManagementSectionService extends Service<ManagementSection> {
    constructor() {
        super('/management/section', 'managementSectionID')
    }

    getAllByProject(managementProjectID: number) {
        return this.getAll('', {
            managementProjectID: managementProjectID,
        });
    }
}