import { useState, useEffect } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import '../details/details.css';
import 'react-toastify/dist/ReactToastify.css';
import { BeatLoader } from "react-spinners";
import { smetaService } from '../../services/smetaService';
import { materialService } from '../../services/materialService';
import Select from 'react-select';
import { Modal, Button, Form } from "react-bootstrap";


const Smeta = ({ workId }) => {

  const [smetas, setSmetas] = useState(null);
  const [allDataLoad, setAllDataLoad] = useState(false);
  const [show, setShow] = useState(false);

  const fetch = () => {
    setAllDataLoad(false);
    smetaService.getByWorkId(workId)
      .then(response => {
        setSmetas(response || []);
      })
      .catch(error => {
        toast.error('Ошибка при загрузке смет');
      })
      .finally(() => {
        setAllDataLoad(true);
      });
  }

  useEffect(() => {
    fetch();
  }, []);


  const SmetaModal = ({ show, onClose, refreshSmeta }) => {

    const [materials, setMaterials] = useState(null);
    const [newSmeta, setNewSmeta] = useState({});
    const [materialDescription, setMaterialDescription] = useState(null);
    const [allMaterialsLoad, setAllMaterialsLoad] = useState(false);
    const [materialCount, setMaterialCount] = useState(1);
    const [materialPrice, setMaterialPrice] = useState(0);
    const [materialsForSelect, setMaterialsForSelect] = useState(null);

    const onMaterialChange = (selected) => {
      setNewSmeta(prev => ({
        ...prev,
        materialId: selected.value
      }));

      const material = materials.find(x => x.id == selected.value);
      setMaterialDescription(material.description);
      setMaterialPrice(material.price);
    };

    const prepareMaterials = (materials) => {
      const prepared = materials.map(item => ({
        value: item.id,
        label: item.name
      }));

      setMaterialsForSelect(prepared);
    };

    const saveNewSmeta = () => {
      newSmeta.workId = workId;
      newSmeta.count = materialCount;
      newSmeta.price = materialPrice;

      smetaService.addSmetaToWork(newSmeta)
      .then(response => {
        refreshSmeta();
      })
      .catch(error => {
        toast.error('Ошибка при сохранении сметы');
      })
      .finally(() => {
        onClose();
      });
      console.log("newSmeta");
      console.log(newSmeta);
    }

    useEffect(() => {
      if (!show) return; // ✅ проверка внутри useEffect

      setAllMaterialsLoad(false);

      materialService.getAll()
        .then(response => {
          setMaterials(response || []);
          prepareMaterials(response)
        })
        .catch(() => {
          toast.error('Ошибка при загрузке смет');
        })
        .finally(() => {
          setAllMaterialsLoad(true);
        });

    }, [show, workId]);

    return (
      <Modal show={show} onHide={onClose} centered>

        {!allMaterialsLoad ? <div className="d-flex justify-content-center align-items-center" style={{ height: "10vh" }}>
          <BeatLoader color="#0d6efd" size={15} />
        </div> : <>
          <Modal.Header closeButton>
            <Modal.Title>Добавить смету</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <Form>

              <Form.Group className="mb-3">
                <Form.Label>Материалы</Form.Label>
                <Select placeholder="Выберите материал" options={materialsForSelect} value={materialsForSelect.find(option => option.value === newSmeta?.materialId) || null} onChange={onMaterialChange} />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Описание материала</Form.Label>
                <Form.Control disabled={true} value={materialDescription} type="text" placeholder='Описание материала' />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label>Количество</Form.Label>
                <Form.Control value={materialCount} onChange={(e) => setMaterialCount(Number(e.target.value))} type="number" />
              </Form.Group>

              <Form.Group>
                <Form.Group>
                  <Form.Label>
                    Итого: {materialPrice * materialCount} ₽
                  </Form.Label>
                </Form.Group>
              </Form.Group>
            </Form>
          </Modal.Body>

          <Modal.Footer>
            <Button variant="primary" onClick={() => saveNewSmeta()}>
              Добавить
            </Button>
          </Modal.Footer>
        </>}
      </Modal>
    )

  }

  if (!allDataLoad) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "10vh" }}>
        <BeatLoader color="#0d6efd" size={15} />
      </div>
    );
  }



  return (
    <div className='border rounded-4 shadow-sm p-3 bg-white'>
      <h5 className="mb-3">Смета:</h5>
      <div className="border rounded-4 shadow-sm" style={{ maxHeight: "120px", overflowY: "auto", backgroundColor: "#fff" }}>
        <div className="table-responsive">
          <table className="table table-sm table-hover align-middle mb-0">
            <thead className="table-light" style={{ position: "sticky", top: 0, zIndex: 1 }}>
              <tr>
                <th>№</th>
                <th>Название</th>
                <th>Описание</th>
                <th className="text-center">Кол-во</th>
                <th className="text-center">Стоимость</th>
                <th className="text-end">Итого</th>
              </tr>
            </thead>

            <tbody>
              {smetas?.length > 0 ? (
                smetas.sort((a, b) => a.id - b.id).map((item, index) => (
                  <tr key={item.id || index}>
                    <td>{item.id}</td>
                    <td className="fw-semibold">
                      {item.material.name}
                    </td>
                    <td className="text-muted small">
                      {item.material.description || "—"}
                    </td>
                    <td className="text-center">
                      {item.count}
                    </td>
                    <td className="text-center">
                      {item.price?.toLocaleString("ru-RU")} ₽
                    </td>
                    <td className="text-end fw-semibold">
                      {(item.count * item.price)?.toLocaleString("ru-RU")} ₽
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan="6" className="text-center text-muted py-3">
                    Нет данных
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
      <button type="button" className="btn btn-outline-primary btn-sm px-4 d-block mx-auto mt-3" style={{ marginTop: '10px' }} onClick={() => (setShow(true))}>
        <i className="bi bi-check-all me-2"></i>Добавить смету
      </button>
      <SmetaModal show={show} refreshSmeta={() => fetch()} onClose={() => (setShow(false))} />
    </div>
  )
};

export default Smeta;