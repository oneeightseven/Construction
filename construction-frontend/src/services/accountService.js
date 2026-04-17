import { apiService } from './apiService.js';

class AccountService {
    async getByWorkId(workId) {
        return await apiService.post('Account/GetByWorkId', workId);
    }

    async addAccountToWork(account){
        return await apiService.post('Account/AddAccountToWork', account);
    }

    async removeAccountById(accountId){
        return await apiService.post('Account/RemoveAccountById', accountId);
    }
}

export const accountService = new AccountService();