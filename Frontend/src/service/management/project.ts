import Service from '../service';
import type { ManagementSectionFull } from '@/service/management/section'

export interface ManagementProject {
    managementProjectID: number
    created: Date
    deleted: Date
    name: string
    backendUserID_CreatedBy: number
}
export interface ManagementProjectFull extends ManagementProject {
    sections: Array<ManagementSectionFull>
}

export default class ManagementProjectService extends Service<ManagementProject> {
    constructor() {
        super('/management/project', 'managementProjectID')
    }

    getFullByProject(managementProjectID: number) {
        return this.get<ManagementProjectFull>(`${managementProjectID}`)
    }
}