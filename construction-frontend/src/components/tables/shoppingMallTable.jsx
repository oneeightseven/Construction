import React, { useState, useEffect } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './constructionObjectsTable.css';
import { shoppingMallService } from '../../services/shoppingMallService';
import ShoppingMallForm from '../forms/shoppingMallForm';

const ShoppingMallTable = () => {
  const [objects, setObjects] = useState(null);
  const [loading, setLoading] = useState(true);

  const [isUpdateMode, setIsUpdateMode] = useState(false);
  const [updateModel, setUpdateModel] = useState(null);

  useEffect(() => {
    fetchObjects();
  }, [isUpdateMode]);

  const fetchObjects = () => {
    setLoading(true);
    shoppingMallService.getAll()
      .then(response => {
        setObjects(response || []);
      })
      .catch(error => {
        toast.error('Ошибка при загрузке торговых центров');
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleDelete = (id, name) => {
    if (window.confirm(`Вы уверены, что хотите удалить торговый центр "${name}"?`)) {
      shoppingMallService.delete(id)
        .then(() => {
          setObjects(prev => prev.filter(obj => obj.id !== id));
          toast.success(`Объект "${name}" успешно удален`);
        })
        .catch(error => {
          console.error('Ошибка при удалении торгового центра:', error);
          toast.error('Ошибка при удалении торгового центра');
        });
    }
  };

  const handleClick = (object) => {
    setIsUpdateMode(true);
    setUpdateModel(object);
  };

  const handleAddObject = () => {
    setUpdateModel(null);
    setIsUpdateMode(true);
  };

  const closeUpdateModal = () => {
    setIsUpdateMode(false);
  };

  if (!isUpdateMode) {
    if (loading) {
      return (
        <div className="container mt-4">
          <div className="row">
            <div className="col-12 text-center">
              <div className="spinner-border text-primary" role="status">
                <span className="visually-hidden">Загрузка...</span>
              </div>
            </div>
          </div>
        </div>
      );
    }

    return (
      <div className="construction-objects-container">
        <div className="container mt-4">
          <div className="row mb-4">
            <div className="col-12">
              <div className="d-flex justify-content-between align-items-center">
                <h2 className="mb-0">Торговые центры</h2>
                <button className="btn btn-primary btn-add" onClick={handleAddObject}>
                  <i className="bi bi-plus-circle me-2"></i>
                  Добавить ТЦ
                </button>
              </div>
            </div>
          </div>

          <div className="row">
            <div className="col-12">
              <div className="construction-table card shadow-sm">
                <div className="card-body p-0">
                  {objects.length === 0 ? (
                    <div className="text-center py-5">
                      <i className="bi bi-inbox display-1 text-muted"></i>
                      <p className="mt-3 text-muted">Нет торговых центров</p>
                    </div>
                  ) : (
                    <div className="table-responsive">
                      {objects.map((object) => (
                        <div key={object.id} className="construction-row row align-items-center py-3 border-bottom" onDoubleClick={() => handleClick(object)}>
                          <div className="col-1 text-center">
                            <span className="object-id badge bg-light text-dark">
                              #{object.id}
                            </span>
                          </div>
                          <div className="col-8">
                            <h6 className="mb-0 object-name">{object.name}</h6>
                          </div>
                          <div className="col-3 text-end">
                            <button className="btn btn-outline-danger btn-sm" onClick={() => handleDelete(object.id, object.name)} title="Удалить ТЦ">
                              <i className="bi bi-trash"></i>
                              Удалить
                            </button>
                          </div>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
  else {
    return <ShoppingMallForm object={updateModel} onCancel={closeUpdateModal}/>
  }
};

export default ShoppingMallTable;