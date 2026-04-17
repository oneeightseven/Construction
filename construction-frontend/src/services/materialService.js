import { apiService } from './apiService.js';

class MaterialService {
    async getAll() {
        return await apiService.get('Material/GetAll');
    }
}

export const materialService = new MaterialService();