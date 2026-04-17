import { apiService } from './apiService.js';

class ConstructionObjectService {
    async getAll() {
        return await apiService.get('ConstructionObject/GetAll');
    }

    async update(obj) {
        return await apiService.post('ConstructionObject/Update', obj);
    }

    async delete(id) {
        return await apiService.post('ConstructionObject/Delete', id);
    }
}

export const constructionObjectService = new ConstructionObjectService();