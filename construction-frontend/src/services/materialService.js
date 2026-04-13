import { apiService } from './apiService.js';

class MaterialService {
    async getAll() {
        return await apiService.get('Home/GetAllMaterials');
    }
}

export const materialService = new MaterialService();