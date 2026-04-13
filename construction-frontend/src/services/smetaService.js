import { apiService } from './apiService.js';

class SmetaService {
    async getByWorkId(workId) {
        return await apiService.post('Home/GetWorkSmetaById', workId);
    }

    async addSmetaToWork(smeta){
        return await apiService.post('Home/AddSmetaToWork', smeta);
    }
}

export const smetaService = new SmetaService();