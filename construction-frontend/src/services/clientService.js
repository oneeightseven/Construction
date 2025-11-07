import { apiService } from './apiService.js';

class ClientService {
    async getAll() {
        return await apiService.get('Home/GetClients');
    }
}

export const clientService = new ClientService();