import { apiService } from './apiService.js';

class WorkService {
    async getAll() {
        return await apiService.get('Home/GetWorks');
    }
}

export const workService = new WorkService();