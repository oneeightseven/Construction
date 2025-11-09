import { apiService } from './apiService.js';

class ShoppingMallService {
    async getAll() {
        return await apiService.get('Home/GetShoppingMalls');
    }

    async update(obj) {
        return await apiService.post('Home/UpdateShoppingMall', obj);
    }

    async delete(id) {
        return await apiService.post('Home/DeleteShoppingMall', id);
    }
}

export const shoppingMallService = new ShoppingMallService();