import axios from "axios";
import { apiService } from './apiService.js';

class AppendicesService {
  async create(file, name, date, work) {
    const formData = new FormData();

    formData.append("File", file);
    formData.append("Name", name);
    formData.append("Date", date);
    formData.append("WorkId", work.workId);

    const { data } = await axios.post(
      "http://localhost:5166/Appendices/Upload",
      formData
    );

    return data;
  }

  async getByWorkId(workId) {
    return await apiService.post('Appendices/GetByWorkId', workId);
  }

  async removeById(id) {
    return await apiService.post('Appendices/RemoveById', id);
  }

  async download(id) {
    return await axios({
      method: "post",
      url: "http://localhost:5166/Appendices/DownloadFile",
      data: JSON.stringify(id),
      headers: {
        "Content-Type": "application/json"
      },
      responseType: "blob"
    });
  }
}

export const appendicesService = new AppendicesService();