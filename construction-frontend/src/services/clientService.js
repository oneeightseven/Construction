import { apiService } from './apiService.js';

class ClientService {
    async getAll() {
        return await apiService.get('Client/GetAll');
    }
     async update(obj) {
        return await apiService.post('Client/Update', obj);
    }

    async delete(id) {
        return await apiService.post('Client/Delete', id);
    }
}

export const clientService = new ClientService();