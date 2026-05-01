import { useState, useEffect } from 'react';
import { Modal, Button, Form } from "react-bootstrap";
import { toast } from 'react-toastify';
import { BeatLoader } from "react-spinners";
import '../details/details.css';
import 'react-toastify/dist/ReactToastify.css';
import { appendicesService } from '../../services/appendicesService';
import png from '../../images/png.png';
import excel from '../../images/excel.png';
import pdf from '../../images/pdf.png';
import word from '../../images/word.png';
import jpeg from '../../images/jpeg.png';
import file from '../../images/file.png';

const Appendices = (workId) => {
  const [appendices, setAppendices] = useState([]);
  const [allDataLoad, setAllDataLoad] = useState(false);

  const [show, setShow] = useState(false);
  const [selectedFile, setSelectedFile] = useState(null);

  const fetchData = () => {
    setAllDataLoad(false);

    appendicesService.getByWorkId(workId.workId)
      .then(response => {
        setAppendices(response || []);
      })
      .catch(() => toast.error("Ошибка загрузки счетов"))
      .finally(() => setAllDataLoad(true));
  }

  useEffect(() => {
    fetchData();
  }, []);

  const getFileImage = (contentType) => {
    if (!contentType) return file;

    if (contentType.includes("png")) return png;
    if (contentType.includes("jpeg")) return jpeg;
    if (contentType.includes("pdf")) return pdf;
    if (contentType.includes("word")) return word;
    if (contentType.includes("sheet")) return excel;

    return file;
  };

  const handleAdd = () => {
    setSelectedFile(null);
    setShow(true);
  };

  const handleClose = () => {
    setShow(false);
    setSelectedFile(null);
  };

  const deleteFile = (id) => {
    if (!window.confirm("Удалить файл?")) return;

    appendicesService.removeById(id)
      .then(response => {
        fetchData();
      })
      .catch(() => toast.error("Ошибка удаления"))
      .finally(() => toast.success("Файл удалён"));

  };

  const getFileNameFromDisposition = (disposition) => {
    if (!disposition) return "download";

    // 1️⃣ сначала filename*
    const fileNameStarMatch = disposition.match(/filename\*\=UTF-8''([^;]+)/i);
    if (fileNameStarMatch?.[1]) {
      return decodeURIComponent(fileNameStarMatch[1]);
    }

    // 2️⃣ потом обычный filename
    const fileNameMatch = disposition.match(/filename="?([^";]+)"?/i);
    if (fileNameMatch?.[1]) {
      return fileNameMatch[1];
    }

    return "download";
  };

  const downloadFile = async (id) => {
    const response = await appendicesService.download(id);

    const blob = response.data;
    const disposition = response.headers["content-disposition"];

    const fileName = getFileNameFromDisposition(disposition);

    const url = window.URL.createObjectURL(blob);

    const a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    document.body.appendChild(a);
    a.click();
    a.remove();

    window.URL.revokeObjectURL(url);
  };

  const FileModal = ({ show, onClose }) => {

    const [file, setFile] = useState(null);
    const [fileName, setFileName] = useState("");
    const [fileType, setFileType] = useState("");
    const [date, setDate] = useState("");

    useEffect(() => {
      if (!show) return;

      const now = new Date().toISOString().slice(0, 16);
      setDate(now);
    }, [show]);

    const getFileType = (fileName) => {
      const extension = fileName.split('.').pop().toLowerCase();

      const documents = ['doc', 'docx', 'pdf', 'txt'];
      const images = ['png', 'jpg', 'jpeg', 'gif', 'bmp', 'webp'];
      const tables = ['xls', 'xlsx', 'csv'];

      if (documents.includes(extension)) return 'Документ';
      if (images.includes(extension)) return 'Изображение';
      if (tables.includes(extension)) return 'Таблица';

      return 'Документ';
    };

    const handleFileChange = (e) => {
      const selected = e.target.files[0];
      if (!selected) return;

      setFile(selected);
      setFileName(selected.name);
      setFileType(getFileType(selected.name));
    };

    const saveFile = async () => {
      if (!file) {
        toast.error("Выберите файл");
        return;
      }

      try {
        const result = await appendicesService.create(
          file,
          fileName,
          date,
          workId
        );

        fetchData();

        toast.success("Файл загружен");
        onClose();

      } catch (e) {
        toast.error("Ошибка загрузки");
      }
    };
    return (
      <Modal show={show} onHide={onClose} centered>
        <Modal.Header closeButton>
          <Modal.Title>Добавить файл</Modal.Title>
        </Modal.Header>

        <Modal.Body>
          <Form>

            <Form.Group className="mb-3">
              <Form.Label>Файл</Form.Label>
              <Form.Control
                type="file"
                onChange={handleFileChange}
              />
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Название файла</Form.Label>
              <Form.Control
                value={fileName}
                onChange={(e) => setFileName(e.target.value)}
              />
            </Form.Group>

            <Form.Group className="mb-3">
              <Form.Label>Тип файла</Form.Label>
              <Form.Control
                value={fileType}
                readOnly
              />
            </Form.Group>

            <Form.Group>
              <Form.Label>Дата добавления</Form.Label>
              <Form.Control
                type="datetime-local"
                value={date}
                onChange={(e) => setDate(e.target.value)}
              />
            </Form.Group>

          </Form>
        </Modal.Body>

        <Modal.Footer>
          <Button variant="secondary" onClick={onClose}>
            Отмена
          </Button>
          <Button variant="primary" onClick={saveFile}>
            Добавить
          </Button>
        </Modal.Footer>
      </Modal>
    );
  };

  if (!allDataLoad) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "10vh" }}>
        <BeatLoader color="#0d6efd" size={15} />
      </div>
    );
  }

  return (
    <div className='border rounded-4 shadow-sm p-3 bg-white' style={{ marginTop: '20px' }}>
      <h5 className="mb-3">Приложения к договору:</h5>

      <div className="border rounded-4 shadow-sm" style={{ maxHeight: "120px", overflowY: "auto" }}>
        <div className="table-responsive">
          <table className="table table-sm table-hover align-middle mb-0">
            <thead className="table-light" style={{ position: "sticky", top: 0 }}>
              <tr>
                <th>№</th>
                <th className='text-center'>Название файла</th>
                <th className='text-center'>Тип</th>
                <th className='text-end'>Дата добавления</th>
                <th></th>
                <th></th>
              </tr>
            </thead>

            <tbody>
              {appendices.length > 0 ? (
                appendices.map((item, index) => (
                  <tr key={item.id}>
                    <td>{index + 1}</td>
                    <td className="fw-semibold text-center">
                      {item.originalFileName}
                    </td>
                    <td className='text-center'>
                      <img src={getFileImage(item.contentType)} alt="file" />
                    </td>
                    <td className='text-end'>
                      {item.date}
                    </td>
                    <td className="text-end">
                      <Button
                        variant="outline-danger"
                        size="sm"
                        onClick={() => deleteFile(item.id)}
                      >
                        Удалить
                      </Button>
                    </td>
                    <td className="text-start">
                      <Button
                        variant="outline-success"
                        size="sm"
                        onClick={() => downloadFile(item.id)}
                      >
                        Скачать
                      </Button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5" className="text-center text-muted py-3">
                    Нет файлов
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      <button
        type="button"
        className="btn btn-outline-primary btn-sm px-4 d-block mx-auto mt-3"
        onClick={handleAdd}
      >
        Добавить файл
      </button>

      <FileModal
        show={show}
        onClose={handleClose}
      />
    </div>
  );
};

export default Appendices;