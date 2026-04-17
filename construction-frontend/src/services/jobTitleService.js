import { apiService } from './apiService.js';

class JobTitleService {
    async getAll() {
        return await apiService.get('JobTitle/GetAll');
    }

    async create(jobTitle) {
        return await apiService.post('JobTitle/CreateTitle', jobTitle);
    }
}

export const jobTitleService = new JobTitleService();