import { apiService } from './apiService.js';

class SmetaService {
    async getByWorkId(workId) {
        return await apiService.post('WorkSmeta/GetByWorkId', workId);
    }

    async addSmetaToWork(smeta){
        return await apiService.post('WorkSmeta/AddToWork', smeta);
    }

    async removeSmetaById(smetaId){
        return await apiService.post('WorkSmeta/RemoveById', smetaId);
    }
}

export const smetaService = new SmetaService();