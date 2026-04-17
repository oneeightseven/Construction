import { apiService } from './apiService.js';

class StatusService {
    async getAll() {
        return await apiService.get('Status/GetAll');
    }
}

export const statusService = new StatusService();