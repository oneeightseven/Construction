import { apiService } from './apiService.js';

class ShoppingMallService {
    async getAll() {
        return await apiService.get('Home/GetShoppingMalls');
    }
}

export const shoppingMallService = new ShoppingMallService();