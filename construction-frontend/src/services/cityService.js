import { apiService } from './apiService.js';

class CityService {
    async getAll() {
        return await apiService.get('City/GetAll');
    }
}

export const cityService = new CityService();