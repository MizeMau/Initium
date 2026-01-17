import Service from '../service';

export interface ManagementProject {
    managementProjectID: number
    created: Date
    deleted: Date
    name: string
    backendUserID_CreatedBy: number
}

export default class ManagementProjectService extends Service<ManagementProject> {
    constructor() {
        super('/management/project', 'managementProjectID')
    }
}