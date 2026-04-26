import { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import { BeatLoader } from "react-spinners";
import { Modal, Button, Form } from "react-bootstrap";
import { accountService } from '../../services/accountService';
import { typeOfAppointmentService } from '../../services/typeOfAppointmentService';
import { clientService } from '../../services/clientService';
import '../details/details.css';
import 'react-toastify/dist/ReactToastify.css';
import Select from 'react-select';

const Accounts = ({ workId }) => {

  const [accounts, setAccounts] = useState([]);
  const [allDataLoad, setAllDataLoad] = useState(false);
  const [show, setShow] = useState(false);
  const [selectedAccount, setSelectedAccount] = useState(null);

  const fetchData = () => {
    setAllDataLoad(false);

    accountService.getByWorkId(workId)
      .then(response => {
        setAccounts(response || []);
      })
      .catch(() => toast.error("Ошибка загрузки счетов"))
      .finally(() => setAllDataLoad(true));
  };

  useEffect(() => {
    fetchData();
  }, [workId]);

  const handleAdd = () => {
    setSelectedAccount(null);
    setShow(true);
  };

  const handleEdit = (item) => {
    setSelectedAccount(item);
    setShow(true);
  };

  const handleClose = () => {
    setShow(false);
    setSelectedAccount(null);
  };

  const AccountModal = ({ show, onClose, refresh, account }) => {

    const isEdit = !!account;

    const [types, setTypes] = useState([]);
    const [typesForSelect, setTypesForSelect] = useState([]);

    const [payers, setPayers] = useState([]);
    const [payersForSelect, setPayersForSelect] = useState([]);

    const [typeId, setTypeId] = useState(null);
    const [payerId, setPayerId] = useState(null);

    const [date, setDate] = useState("");
    const [sum, setSum] = useState(0);
    const [details, setDetails] = useState("");

    const [loading, setLoading] = useState(false);

    useEffect(() => {
      if (!show) return;

      setLoading(true);

      Promise.all([
        typeOfAppointmentService.getAll(),
        clientService.getAll()
      ])
        .then(([typesRes, payersRes]) => {

          const typesData = typesRes || [];
          const payersData = payersRes || [];

          setTypes(typesData);
          setPayers(payersData);

          setTypesForSelect(
            typesData.map(x => ({
              value: x.id,
              label: x.name
            }))
          );

          setPayersForSelect(
            payersData.map(x => ({
              value: x.id,
              label: x.name
            }))
          );
        })
        .catch(() => toast.error("Ошибка загрузки данных"))
        .finally(() => setLoading(false));

    }, [show]);

    useEffect(() => {
      if (!account) return;

      setTypeId(account.typeOfAppointment?.id);
      setPayerId(account.payer?.id);
      setDate(account.date?.slice(0, 16));
      setSum(account.sum);
      setDetails(account.details);

    }, [account]);

    const saveAccount = () => {

      if (!typeId || !payerId || !date) {
        toast.error("Заполните все поля");
        return;
      }

      const payload = {
        id: account?.id,
        workId: workId,
        typeOfAppointmentId: typeId,
        payerId: payerId,
        date: date,
        sum: sum,
        details: details
      };

      accountService.addAccountToWork(payload)
        .then(() => {
          toast.success(isEdit ? "Счёт обновлён" : "Счёт добавлен");
          refresh();
        })
        .catch(() => toast.error("Ошибка при сохранении"))
        .finally(() => onClose());
    };

    const deleteAccount = () => {
      if (!account?.id) return;
      if (!window.confirm("Удалить счёт?")) return;

      accountService.removeAccountById(account.id)
        .then(() => {
          toast.success("Счёт удалён");
          refresh();
        })
        .catch(() => toast.error("Ошибка при удалении"))
        .finally(() => onClose());
    };

    return (
      <Modal show={show} onHide={onClose} centered>

        {loading ? (
          <div className="d-flex justify-content-center align-items-center" style={{ height: "15vh" }}>
            <BeatLoader color="#0d6efd" />
          </div>
        ) : (
          <>
            <Modal.Header closeButton>
              <Modal.Title>
                {isEdit ? "Редактировать счёт" : "Добавить счёт"}
              </Modal.Title>
            </Modal.Header>

            <Modal.Body>
              <Form>

                <Form.Group className="mb-3">
                  <Form.Label>Назначение</Form.Label>
                  <Select
                    options={typesForSelect}
                    value={typesForSelect.find(x => x.value === typeId) || null}
                    onChange={(selected) => setTypeId(selected.value)}
                    placeholder="Выберите назначение"
                  />
                </Form.Group>

                <Form.Group className="mb-3">
                  <Form.Label>Дата</Form.Label>
                  <Form.Control
                    type="datetime-local"
                    value={date}
                    onChange={(e) => setDate(e.target.value)}
                  />
                </Form.Group>

                <Form.Group className="mb-3">
                  <Form.Label>Плательщик</Form.Label>
                  <Select
                    options={payersForSelect}
                    value={payersForSelect.find(x => x.value === payerId) || null}
                    onChange={(selected) => setPayerId(selected.value)}
                    placeholder="Выберите плательщика"
                  />
                </Form.Group>

                <Form.Group>
                  <Form.Label>Сумма</Form.Label>
                  <Form.Control
                    type="number"
                    value={sum}
                    onChange={(e) => setSum(Number(e.target.value))}
                  />
                </Form.Group>

                <Form.Group>
                  <Form.Label>Реквизиты</Form.Label>
                  <Form.Control
                    value={details}
                    onChange={(e) => setDetails(e.target.value)}
                  />
                </Form.Group>

              </Form>
            </Modal.Body>

            <Modal.Footer>
              {isEdit && (
                <Button variant="danger" onClick={deleteAccount}>
                  Удалить
                </Button>
              )}
              <div className="ms-auto">
                <Button variant="primary" onClick={saveAccount}>
                  {isEdit ? "Сохранить" : "Добавить"}
                </Button>
              </div>
            </Modal.Footer>
          </>
        )}
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
      <h5 className="mb-3">Счета:</h5>

      <div className="border rounded-4 shadow-sm" style={{ maxHeight: "120px", overflowY: "auto" }}>
        <div className="table-responsive">
          <table className="table table-sm table-hover align-middle mb-0">
            <thead className="table-light" style={{ position: "sticky", top: 0 }}>
              <tr>
                <th>№</th>
                <th className='text-center'>Назначение</th>
                <th className='text-center'>Дата</th>
                <th className='text-center'>Плательщик</th>
                <th className='text-end'>Сумма</th>
              </tr>
            </thead>
            <tbody>
              {accounts.length > 0 ? (
                accounts.sort((a, b) => a.id - b.id).map(item => (
                  <tr key={item.id} style={{ cursor: "pointer" }} onClick={() => handleEdit(item)}>
                    <td>{item.id}</td>
                    <td className="fw-semibold text-center">{item.typeOfAppointment.name}</td>
                    <td className='text-center'>
                      {new Date(item.date).toLocaleString("ru-RU", {
                        day: "2-digit",
                        month: "2-digit",
                        year: "numeric",
                        hour: "2-digit",
                        minute: "2-digit"
                      })}
                    </td>
                    <td className='text-center'>{item.payer.name}</td>
                    <td className='text-end fw-semibold'>
                      {Number(item.sum).toLocaleString("ru-RU")} ₽
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="5" className="text-center text-muted py-3">
                    Нет счетов
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
        Добавить счёт
      </button>

      <AccountModal
        show={show}
        onClose={handleClose}
        refresh={fetchData}
        account={selectedAccount}
      />
    </div>
  );
};

export default Accounts;