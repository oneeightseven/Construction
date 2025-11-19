import React, { useState, useEffect } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import './clientsTable.css';
import { clientService } from '../../services/clientService';
import ClientForm from '../forms/clientForm';

const ClientsTable = () => {
  const [clients, setClients] = useState(null);
  const [loading, setLoading] = useState(true);

  const [isUpdateMode, setIsUpdateMode] = useState(false);
  const [updateModel, setUpdateModel] = useState(null);

  useEffect(() => {
    fetchClients();
  }, [isUpdateMode]);

  const fetchClients = () => {
    setLoading(true);
    clientService.getAll()
      .then(response => {
        setClients(response || []);
      })
      .catch(error => {
        toast.error('Ошибка при загрузке клиентов');
      })
      .finally(() => {
        setLoading(false);
      });
  };

  const handleDelete = (id, name) => {
    if (window.confirm(`Вы уверены, что хотите удалить клиента "${name}"?`)) {
      clientService.delete(id)
        .then(() => {
          setClients(prev => prev.filter(obj => obj.id !== id));
          toast.success(`Клиент "${name}" успешно удален`);
        })
        .catch(error => {
          console.error('Ошибка при удалении клиента:', error);
          toast.error('Ошибка при удалении клиента');
        });
    }
  };

  const handleClick = (client) => {
    setIsUpdateMode(true);
    setUpdateModel(client);
  };

  const handleAddClient = () => {
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
      <div className="clients-container">
        <div className="container mt-4">
          <div className="row mb-4">
            <div className="col-12">
              <div className="d-flex justify-content-between align-items-center">
                <h2 className="mb-0">Клиенты</h2>
                <button className="btn btn-primary btn-add" onClick={handleAddClient}>
                  <i className="bi bi-plus-circle me-2"></i>
                  Добавить клиента
                </button>
              </div>
            </div>
          </div>

          <div className="row">
            <div className="col-12">
              <div className="client-table card shadow-sm">
                <div className="card-body p-0">
                  {clients.length === 0 ? (
                    <div className="text-center py-5">
                      <i className="bi bi-inbox display-1 text-muted"></i>
                      <p className="mt-3 text-muted">Нет клиентов</p>
                    </div>
                  ) : (
                    <div className="table-responsive">
                      {clients.map((client) => (
                        <div key={client.id} className="client-row row align-items-center py-3 border-bottom" onDoubleClick={() => handleClick(client)}>
                          <div className="col-1 text-center">
                            <span className="client-id badge bg-light text-dark">
                              #{client.id}
                            </span>
                          </div>
                          <div className="col-8">
                            <h6 className="mb-0 client-name">{client.name}</h6>
                          </div>
                          <div className="col-3 text-end">
                            <button className="btn btn-outline-danger btn-sm" onClick={() => handleDelete(client.id, client.name)} title="Удалить клиента">
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
    return <ClientForm client={updateModel} onCancel={closeUpdateModal}/>
  }
};

export default ClientsTable;
