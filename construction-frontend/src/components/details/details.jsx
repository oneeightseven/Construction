import { useState, useEffect } from 'react';
import Select from 'react-select';
import { statusService } from '../../services/statusService.js';
import { clientService } from '../../services/clientService.js';
import { cityService } from '../../services/cityService.js';
import { shoppingMallService } from '../../services/shoppingMallService.js';
import { workService } from '../../services/workService.js';
import '../details/details.css';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import { BeatLoader } from "react-spinners";
import Smeta from '../smeta/smeta.jsx';
import Appendices from '../appendices/appendices.jsx';
import Accounts from '../accounts/accounts.jsx';



const Details = ({ work, exitFunc }) => {

  const [statuses, setStatuses] = useState(null);
  const [currentStatus, setCurrentStatus] = useState(null);
  const [clients, setClients] = useState(null);
  const [currentClient, setCurrentClient] = useState(null);
  const [cities, setCities] = useState(null);
  const [currentCity, setCurrentCity] = useState(null);
  const [shoppingMalls, setShoppingMalls] = useState(null);
  const [currentShoppingMall, setCurrentShoppingMall] = useState(null);
  const [estimates, setEstimates] = useState(null);
  const [allDataLoad, setAllDataLoad] = useState(false);

  const [totalEstimate, setTotalEstimate] = useState(150000);

  const addTotals = () => {
    let objects = [];
    let moq1 = { id: 0, name: 'Ламинат (1х1)', description: 'Черный', price: 1000, quantity: 5 }
    let moq2 = { id: 0, name: 'Обои (2x2)', description: 'Белые', price: 1500, quantity: 10 }
    let moq3 = { id: 0, name: 'Гвозди (100гм)', description: 'Б/У', price: 150, quantity: 130 }
    let moq4 = { id: 0, name: 'Клей (1л)', description: 'Розовый', price: 300, quantity: 3 }
    objects.push(moq1);
    objects.push(moq2);
    objects.push(moq3);
    objects.push(moq4);
    setEstimates(objects);
  }

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
    addTotals();

    setAllDataLoad(true);
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

  const handleClose = () => {
    exitFunc();
  };

  const handleSave = (shouldClose) => {
    workService.update(work)
      .then(response => {
        toast.success('Данные успешно сохранены!');
        if (shouldClose) {
          exitFunc();
        }
      })
      .catch(error => {
        toast.error('Ошибка при сохранении: ' + error.message);
      });
  };

  if (!allDataLoad) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "60vh" }}>
        <BeatLoader color="#0d6efd" size={15} />
      </div>
    );
  }


  return (
    <div className="container-fluid form-container">
      <ToastContainer position="top-right" autoClose={2000} />
      <div className='row'>
        <div className="col-6">
          <div className="row mb-4">
            <div className="col-12">
              <div className="d-flex gap-3 justify-content-start">
                <button type="button" className="btn btn-outline-primary btn-lg px-4" onClick={handleClose}>
                  <i className="bi bi-check-circle me-2"></i>Закрыть
                </button>
                <button type="button" className="btn btn-outline-primary btn-lg px-4" onClick={() => handleSave(false)}>
                  <i className="bi bi-check-circle me-2"></i>Сохранить
                </button>
                <button type="button" className="btn btn-primary btn-lg px-4" onClick={() => handleSave(true)}>
                  <i className="bi bi-check-all me-2"></i>Сохранить и закрыть
                </button>
              </div>
            </div>
          </div>


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
                  <i className="bi bi-hash me-2"></i>Объект
                </label>
                <input className="form-control form-control-lg border-accent-primary bg-light" value={work.constructionObject?.name} readOnly />
              </div>
            </div>

            <div className="col-md-6 col-lg-4">
              <div className="form-group">
                <label className="form-label"><i className="bi bi-clock me-2"></i>Срок выполнения</label>
                <input className="form-control form-control-lg border-accent-warning" value={work.dateBid} />
              </div>
            </div>

            <div className="col-md-6 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-building me-2"></i>Контрагент
                </label>
                <div className="react-select-container">
                  <Select value={currentClient} options={clients} isSearchable={true} placeholder="Выберите контрагента..." onChange={(object) => changeWorkField('client', object)} autoFocus menuPlacement="auto" classNamePrefix="react-select" />
                </div>
              </div>
            </div>

            <div className="col-md-6 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-geo-alt me-2"></i>Город
                </label>
                <div className="react-select-container">
                  <Select value={currentCity} options={cities} isSearchable={true} placeholder="Выберите город..." onChange={(object) => changeWorkField('city', object)} menuPlacement="auto" classNamePrefix="react-select" />
                </div>
              </div>
            </div>

            <div className="col-md-6 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-info-circle me-2"></i>Статус
                </label>
                <div className="react-select-container">
                  <Select value={currentStatus} options={statuses} isSearchable={true} placeholder="Выберите статус..." onChange={(object) => changeWorkField('status', object)} menuPlacement="auto" classNamePrefix="react-select" />
                </div>
              </div>
            </div>

            <div className="col-md-6 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-shop me-2"></i>Торговый центр
                </label>
                <div className="react-select-container">
                  <Select value={currentShoppingMall} options={shoppingMalls} isSearchable={true} placeholder="Выберите торговый центр..." onChange={(object) => changeWorkField('shoppingMall', object)} menuPlacement="auto" classNamePrefix="react-select" />
                </div>
              </div>
            </div>

            <div className="col-md-6 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-calendar-check me-2"></i>Дата завершения
                </label>
                <input className="form-control form-control-lg border-accent-success" value={work.completionDate} />
              </div>
            </div>

            <div className="col-md-6 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-person me-2"></i>Менеджер
                </label>
                <input className="form-control form-control-lg border-accent-primary" value={work.employee.surname + " " + work.employee.name + " " + work.employee.patronymic} readOnly />
              </div>
            </div>

            <div className="col-md-4 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-card-text me-2"></i>
                  Описание
                </label>

                <textarea
                  className="form-control form-control-lg border-accent-primary"
                  rows={4}
                />
              </div>
            </div>

            <div className="col-md-4 col-lg-4" style={{ marginTop: '-15px' }}>
              <div className="form-group">
                <label className="form-label">
                  <i className="bi bi-card-text me-2"></i>
                  Примечание
                </label>

                <textarea
                  className="form-control form-control-lg border-accent-primary"
                  rows={4}
                />
              </div>
            </div>

            <div className="col-md-4 col-lg-4" style={{ marginTop: "15px" }}>
              <div className="border rounded p-3 bg-light">

                <div className="d-flex justify-content-between mb-2">
                  <span className="text-muted">Итого по смете:</span>
                  <span className="fw-semibold">
                    {totalEstimate.toLocaleString("ru-RU")} ₽
                  </span>
                </div>

                <div className="d-flex justify-content-between mb-2">
                  <span className="text-muted">Сумма итого:</span>
                  <span className="fw-semibold">
                    {work.summ.toLocaleString("ru-RU")} ₽
                  </span>
                </div>

                <div className="d-flex justify-content-between mb-2">
                  <span className="text-muted">Доход итого:</span>
                  <span className="fw-semibold text-success">
                    {(work.summ - totalEstimate).toLocaleString("ru-RU")} ₽
                  </span>
                </div>

                <div className="d-flex justify-content-between">
                  <span className="text-muted">Общая наценка, %:</span>
                  <span
                    className={`fw-semibold ${totalEstimate > 0 && (work.summ - totalEstimate) >= 0
                      ? "text-success"
                      : "text-danger"
                      }`}
                  >
                    {totalEstimate > 0
                      ? (((work.summ - totalEstimate) / totalEstimate) * 100).toFixed(1)
                      : 0}
                    %
                  </span>
                </div>

              </div>
            </div>

          </div>
        </div>

        <div className="col-6">
          {/* Смета */}
          <Smeta workId={work.id}/>
          {/* Приложения к договору*/}
          <Appendices />
          {/* Счета */}
          <Accounts workId={work.id}/>
        </div>
      </div>
    </div>
  );
};

export default Details;