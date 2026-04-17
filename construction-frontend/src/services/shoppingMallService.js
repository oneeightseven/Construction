import { apiService } from './apiService.js';

class ShoppingMallService {
    async getAll() {
        return await apiService.get('ShoppingMall/GetAll');
    }

    async update(obj) {
        return await apiService.post('ShoppingMall/Update', obj);
    }

    async delete(id) {
        return await apiService.post('ShoppingMall/Delete', id);
    }
}

export const shoppingMallService = new ShoppingMallService();