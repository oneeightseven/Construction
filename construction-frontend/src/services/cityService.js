import { apiService } from './apiService.js';

class CityService {
    async getAll() {
        return await apiService.get('Home/GetCities');
    }
}

export const cityService = new CityService();