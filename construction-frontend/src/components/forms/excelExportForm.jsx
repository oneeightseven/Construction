import React, { useState } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import { excelService } from '../../services/excelService';
import { useBase } from '../contexts/BaseContext';


const ExcelExportForm = ({ }) => {

  const { currentTable, setCurrentTable } = useBase();

  const onCloseModal = () => {
    setCurrentTable(null);
  }

  const [formData, setFormData] = useState({ 
    startDate: '', 
    endDate: '' 
  });
  const [loading, setLoading] = useState(false);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.startDate || !formData.endDate) {
      toast.error('Необходимо заполнить обе даты');
      return;
    }

    if (new Date(formData.startDate) > new Date(formData.endDate)) {
      toast.error('Дата начала не может быть больше даты окончания');
      return;
    }

    setLoading(true);

    excelService.getBlob({
        dateFrom: formData.startDate,
        dateTo: formData.endDate
    })
      .then((blob) => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.style.display = 'none';
        a.href = url;
        a.download = `детализация_${formData.startDate}_${formData.endDate}.xlsx`;
        document.body.appendChild(a);
        a.click();
        window.URL.revokeObjectURL(url);
        document.body.removeChild(a);
        
        toast.success('Выгрузка успешно получена');
        setLoading(false);
      })
      .catch(error => {
        toast.error('Ошибка при получении выгрузки');
        console.error('Export error:', error);
        setLoading(false);
      });
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  return (
    <div className="excel-export-form">
      <ToastContainer position="top-right" autoClose={2000} hideProgressBar={false} newestOnTop={false} closeOnClick rtl={false} pauseOnFocusLoss draggable pauseOnHover theme="light" />
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8 col-lg-6">
            <div className="card shadow-sm">
              <div className="card-header bg-primary text-white">
                <h4 className="mb-0">
                  <i className="bi bi-file-earmark-excel me-2"></i>
                  Выгрузка детализации в Excel
                </h4>
              </div>
              <div className="card-body">
                <form onSubmit={handleSubmit}>
                  <div className="mb-3">
                    <label htmlFor="startDate" className="form-label">
                      Дата начала <span className="text-danger">*</span>
                    </label>
                    <input type="date" className="form-control" id="startDate" name="startDate" value={formData.startDate} onChange={handleChange} disabled={loading}/>
                  </div>

                  <div className="mb-3">
                    <label htmlFor="endDate" className="form-label">
                      Дата окончания <span className="text-danger">*</span>
                    </label>
                    <input type="date" className="form-control" id="endDate" name="endDate" value={formData.endDate} onChange={handleChange} disabled={loading}/>
                  </div>

                  <div className="d-flex gap-2 justify-content-end">
                    <button type="button" className="btn btn-outline-secondary" onClick={onCloseModal} disabled={loading}>
                      <i className="bi bi-x-circle me-2"></i>
                      Отмена
                    </button>
                    <button type="submit" className="btn btn-success" disabled={loading || !formData.startDate || !formData.endDate}>
                      {loading ? (
                        <>
                          <span className="spinner-border spinner-border-sm me-2" role="status"></span>
                          Формирование...
                        </>
                      ) : (
                        <>
                          <i className="bi bi-download me-2"></i>
                          Получить выгрузку
                        </>
                      )}
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ExcelExportForm;