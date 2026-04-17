import { apiService } from './apiService.js';

class AccountService {
    async getByWorkId(workId) {
        return await apiService.post('Home/GetAccountsByWorkId', workId);
    }

    async addAccountToWork(account){
        return await apiService.post('Home/AddAccountToWork', account);
    }

    async removeAccountById(accountId){
        return await apiService.post('Home/RemoveAccountById', accountId);
    }
}

export const accountService = new AccountService();