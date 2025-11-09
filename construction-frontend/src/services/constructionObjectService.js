import { apiService } from './apiService.js';

class ConstructionObjectService {
    async getAll() {
        return await apiService.get('Home/GetConstructionObjects');
    }

    async update(obj) {
        return await apiService.post('Home/UpdateConstructionObject', obj);
    }

    async delete(id) {
        return await apiService.post('Home/DeleteConstructionObject', id);
    }
}

export const constructionObjectService = new ConstructionObjectService();