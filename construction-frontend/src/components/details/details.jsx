import { useState, useEffect } from 'react';
import Select from 'react-select';
import { statusService } from '../../services/statusService.js';
import { clientService } from '../../services/clientService.js';
import { cityService } from '../../services/cityService.js';
import { shoppingMallService } from '../../services/shoppingMallService.js';
import '../details/details.css';



const Details = ({ work, exitFunc }) => {

  const [statuses, setStatuses] = useState(null);
  const [currentStatus, setCurrentStatus] = useState(null);
  const [clients, setClients] = useState(null);
  const [currentClient, setCurrentClient] = useState(null);
  const [cities, setCities] = useState(null);
  const [currentCity, setCurrentCity] = useState(null);
  const [shoppingMalls, setShoppingMalls] = useState(null);
  const [currentShoppingMall, setCurrentShoppingMall] = useState(null);

  const prepareOption = (item, setter) => { setter({ value: item.id, label: item.name }); }

  const loadAllData = async () => {
    const [statuses, clients, cities, shoppingMalls] = await Promise.all([
      statusService.getAll(),
      clientService.getAll(),
      cityService.getAll(),
      shoppingMallService.getAll()
    ]);

    setStatuses(statuses.map(item => ({ value: item.id, label: item.name })));
    setClients(clients.map(item => ({ value: item.id, label: item.name })));
    setCities(cities.map(item => ({ value: item.id, label: item.name })));
    setShoppingMalls(shoppingMalls.map(item => ({ value: item.id, label: item.name })));
  }

  const changeWorkField = (field, object) => {
    const setters = {
      status: setCurrentStatus,
      city: setCurrentCity,
      client: setCurrentClient,
      shoppingMall: setCurrentShoppingMall
    };
    setters[field](object);

    work[field].id = object.value;
    work[field].name = object.label;
  };

  useEffect(() => {
    prepareOption(work.status, setCurrentStatus);
    prepareOption(work.client, setCurrentClient);
    prepareOption(work.city, setCurrentCity);
    prepareOption(work.shoppingMall, setCurrentShoppingMall);


    loadAllData();
  }, []);

  const handleSave = () => {
    exitFunc();
  };

  const handleClose = () => {
    exitFunc();
  };

  const handleSaveAndClose = () => {
    exitFunc();
  };

  return (
    <div className="container-fluid form-container">
      <div className="row mb-4">
        <div className="col-12">
          <div className="d-flex gap-3 justify-content-start">
            <button type="button" className="btn btn-outline-primary btn-lg px-4" onClick={handleClose}>
              <i className="bi bi-check-circle me-2"></i>Закрыть
            </button>
            <button type="button" className="btn btn-outline-primary btn-lg px-4" onClick={handleSave}>
              <i className="bi bi-check-circle me-2"></i>Сохранить
            </button>
            <button type="button" className="btn btn-primary btn-lg px-4" onClick={handleSaveAndClose}>
              <i className="bi bi-check-all me-2"></i>Сохранить и закрыть
            </button>
          </div>
        </div>
      </div>

      <div className="col-12">
        <div className="row g-4">
          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-calendar3 me-2"></i>Дата заявки
              </label>
              <input className="form-control form-control-lg border-accent-primary" value={work.dateOfCreation} readOnly />
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-hash me-2"></i>Номер заявки
              </label>
              <input className="form-control form-control-lg border-accent-primary bg-light" value={work.id} readOnly />
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label"><i className="bi bi-clock me-2"></i>Срок выполнения</label>
              <input className="form-control form-control-lg border-accent-warning" value={work.dateBid} />
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-building me-2"></i>Контрагент
              </label>
              <div className="react-select-container">
                <Select value={currentClient} options={clients} isSearchable={true} placeholder="Выберите контрагента..." onChange={(object) => changeWorkField('client', object)} autoFocus menuPlacement="auto" classNamePrefix="react-select" />
              </div>
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-geo-alt me-2"></i>Город
              </label>
              <div className="react-select-container">
                <Select value={currentCity} options={cities} isSearchable={true} placeholder="Выберите город..." onChange={(object) => changeWorkField('city', object)} menuPlacement="auto" classNamePrefix="react-select" />
              </div>
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-info-circle me-2"></i>Статус
              </label>
              <div className="react-select-container">
                <Select value={currentStatus} options={statuses} isSearchable={true} placeholder="Выберите статус..." onChange={(object) => changeWorkField('status', object)} menuPlacement="auto" classNamePrefix="react-select" />
              </div>
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-shop me-2"></i>Торговый центр
              </label>
              <div className="react-select-container">
                <Select value={currentShoppingMall} options={shoppingMalls} isSearchable={true} placeholder="Выберите торговый центр..." onChange={(object) => changeWorkField('shoppingMall', object)} menuPlacement="auto" classNamePrefix="react-select" />
              </div>
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-calendar-check me-2"></i>Дата завершения
              </label>
              <input className="form-control form-control-lg border-accent-success" value={work.completionDate} />
            </div>
          </div>

          <div className="col-md-6 col-lg-4">
            <div className="form-group">
              <label className="form-label">
                <i className="bi bi-person me-2"></i>Менеджер
              </label>
              <input className="form-control form-control-lg border-accent-primary" value={work.employee.surname + " " + work.employee.name + " " + work.employee.patronymic} readOnly />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Details;