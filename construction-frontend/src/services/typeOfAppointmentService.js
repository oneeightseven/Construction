import { apiService } from './apiService.js';

class TypeOfAppointmentService {
    async getAll() {
        return await apiService.get('TypeOfAppointment/GetAll');
    }
}

export const typeOfAppointmentService = new TypeOfAppointmentService();