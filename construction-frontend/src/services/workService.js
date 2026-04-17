import { apiService } from './apiService.js';

class WorkService {
    async getAll() {
        return await apiService.get('Work/GetAll');
    }

    async update(work) {
        return await apiService.post('Work/Update', work);
    }
}

export const workService = new WorkService();