import { apiService } from './apiService.js';

class StatusService {
    async getAll() {
        return await apiService.get('Home/GetStatuses');
    }
}

export const statusService = new StatusService();