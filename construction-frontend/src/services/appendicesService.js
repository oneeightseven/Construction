import axios from "axios";

class AppendicesService {
  async create(file, name, date) {
    const formData = new FormData();

    // Имена должны совпадать с DTO!
    formData.append("File", file);
    formData.append("Name", name);
    formData.append("Date", date);

    const { data } = await axios.post(
      "http://localhost:5166/Appendices/Upload",
      formData
    );

    return data; // { id: ... }
  }
}

export const appendicesService = new AppendicesService();