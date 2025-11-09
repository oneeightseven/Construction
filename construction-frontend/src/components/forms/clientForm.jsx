import React, { useState, useEffect } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import { clientService } from '../../services/clientService';

const ClientForm = ({ client, onCancel }) => {
  const [formData, setFormData] = useState({ name: '' });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (client) {
      setFormData({
        id: client.id || 0,
        name: client.name || ''
      });
    }
  }, [client]);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.name.trim()) {
      toast.error('Имя клиента не может быть пустым');
      return;
    }

    setLoading(true);

    clientService.update(formData)
      .then(() => {
        setLoading(false);
        onCancel();
      })
      .catch(error => {
        toast.error('Ошибка при сохраненнии клиента', error);
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
    <div className="clients-form">
      <ToastContainer position="top-right" autoClose={2000} hideProgressBar={false} newestOnTop={false} closeOnClick rtl={false} pauseOnFocusLoss draggable pauseOnHover theme="light" />
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8 col-lg-6">
            <div className="card shadow-sm">
              <div className="card-header bg-primary text-white">
                <h4 className="mb-0">
                  <i className={`bi bi-${client ? 'pencil-square' : 'plus-circle'} me-2`}></i>
                  {client ? 'Редактирование клиента' : 'Создание нового клиента'}
                </h4>
              </div>
              <div className="card-body">
                <form onSubmit={handleSubmit}>
                  <div className="mb-3">
                    <label htmlFor="name" className="form-label">
                      Имя клиента <span className="text-danger">*</span>
                    </label>
                    <input type="text" className="form-control" id="name" name="name" value={formData.name} onChange={handleChange} placeholder="Введите имя клиента" disabled={loading} autoFocus/>
                  </div>

                  {client && (
                    <div className="mb-3">
                      <label className="form-label">ID клиента</label>
                      <div className="form-control bg-light">
                        <code>#{client.id}</code>
                      </div>
                      <div className="form-text">
                        Идентификатор клиена нельзя изменить
                      </div>
                    </div>
                  )}

                  <div className="d-flex gap-2 justify-content-end">
                    <button type="button" className="btn btn-outline-secondary" onClick={onCancel} disabled={loading}>
                      <i className="bi bi-x-circle me-2"></i>
                      Отмена
                    </button>
                    <button type="submit" className="btn btn-primary" disabled={loading || !formData.name.trim()}>
                      {loading ? (
                        <>
                          <span className="spinner-border spinner-border-sm me-2" role="status"></span>
                          Сохранение...
                        </>
                      ) : (
                        <>
                          <i className={`bi bi-${client ? 'check' : 'plus'}-circle me-2`}></i>
                          {client ? 'Сохранить изменения' : 'Создать клиента'}
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

export default ClientForm;