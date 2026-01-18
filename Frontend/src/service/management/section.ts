import Service from '../service';

export interface ManagementSection {
    managementSectionID: number
    created: Date
    deleted: Date
    name: string
    managementProjectID: number
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