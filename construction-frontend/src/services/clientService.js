import { apiService } from './apiService.js';

class ClientService {
    async getAll() {
        return await apiService.get('Home/GetClients');
    }
     async update(client) {
        return await apiService.post('Home/UpdateClients', client);
    }

    async delete(id) {
        return await apiService.post('Home/DeleteClients', id);
    }
}

export const clientService = new ClientService();