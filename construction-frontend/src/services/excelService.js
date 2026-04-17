import { apiService } from './apiService.js';

class ExcelService {
    async getBlob(model) {
        return await apiService.postBlob('Excel/Download', model);
    }
}

export const excelService = new ExcelService();