import { apiService } from './apiService.js';

class ClientService {
    async getAll() {
        return await apiService.get('Home/GetClients');
    }
     async update(obj) {
        return await apiService.post('Home/UpdateClient', obj);
    }

    async delete(id) {
        return await apiService.post('Home/DeleteClient', id);
    }
}

export const clientService = new ClientService();