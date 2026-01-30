import Service from '../service';
import type { ManagementTask } from '@/service/management/task'

export interface ManagementSection {
    managementSectionID: number
    created: Date
    deleted: Date
    name: string
    managementProjectID: number
}
export interface ManagementSectionFull extends ManagementSection {
    sections: Array<ManagementTask>
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