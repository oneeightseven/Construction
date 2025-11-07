import { apiService } from './apiService.js';

class JobTitleService {
    async getAll() {
        return await apiService.get('Home/GetTitles');
    }

    async create(jobTitle) {
        return await apiService.post('Home/CreateTitle', jobTitle);
    }
}

export const jobTitleService = new JobTitleService();