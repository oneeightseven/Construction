import { apiService } from './apiService.js';

class ExcelService {
    async getBlob(model) {
        return await apiService.postBlob('Home/DownloadExcel', model);
    }
}

export const excelService = new ExcelService();