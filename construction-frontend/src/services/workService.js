import { apiService } from './apiService.js';

class WorkService {
    async getAll() {
        return await apiService.get('Home/GetWorks');
    }

    async update(work) {
        return await apiService.post('Home/UpdateWork', work);
    }
}

export const workService = new WorkService();