import React, { useState, useEffect } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import { constructionObjectService } from '../../services/constructionObjectService';

const ConstructionObjectForm = ({ object, onCancel }) => {
  const [formData, setFormData] = useState({ name: '' });
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (object) {
      setFormData({
        id: object.id || 0,
        name: object.name || ''
      });
    }
  }, [object]);

  const handleSubmit = (e) => {
    e.preventDefault();

    if (!formData.name.trim()) {
      toast.error('Название объекта не может быть пустым');
      return;
    }

    setLoading(true);

    constructionObjectService.update(formData)
      .then(() => {
        setLoading(false);
        onCancel();
      })
      .catch(error => {
        toast.error('Ошибка при сохраненнии объекта', error);
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
    <div className="construction-object-form">
      <ToastContainer position="top-right" autoClose={2000} hideProgressBar={false} newestOnTop={false} closeOnClick rtl={false} pauseOnFocusLoss draggable pauseOnHover theme="light" />
      <div className="container">
        <div className="row justify-content-center">
          <div className="col-md-8 col-lg-6">
            <div className="card shadow-sm">
              <div className="card-header bg-primary text-white">
                <h4 className="mb-0">
                  <i className={`bi bi-${object ? 'pencil-square' : 'plus-circle'} me-2`}></i>
                  {object ? 'Редактирование объекта' : 'Создание нового объекта'}
                </h4>
              </div>
              <div className="card-body">
                <form onSubmit={handleSubmit}>
                  <div className="mb-3">
                    <label htmlFor="name" className="form-label">
                      Название объекта <span className="text-danger">*</span>
                    </label>
                    <input type="text" className="form-control" id="name" name="name" value={formData.name} onChange={handleChange} placeholder="Введите название строительного объекта" disabled={loading} autoFocus/>
                  </div>

                  {object && (
                    <div className="mb-3">
                      <label className="form-label">ID объекта</label>
                      <div className="form-control bg-light">
                        <code>#{object.id}</code>
                      </div>
                      <div className="form-text">
                        Идентификатор объекта нельзя изменить
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
                          <i className={`bi bi-${object ? 'check' : 'plus'}-circle me-2`}></i>
                          {object ? 'Сохранить изменения' : 'Создать объект'}
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

export default ConstructionObjectForm;