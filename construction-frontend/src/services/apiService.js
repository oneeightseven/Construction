const API_BASE_URL = 'http://localhost:5166/';

class ApiService {
    async get(endpoint) {
        const response = await fetch(`${API_BASE_URL}${endpoint}`);
        return await response.json();
    }

    async post(endpoint, data) {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data)
        });
        return await response.json();
    }
}

export const apiService = new ApiService();