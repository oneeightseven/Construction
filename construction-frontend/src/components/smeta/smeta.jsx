import { useState, useEffect } from 'react';
import { toast } from 'react-toastify';
import { BeatLoader } from "react-spinners";
import { smetaService } from '../../services/smetaService';
import { materialService } from '../../services/materialService';
import Select from 'react-select';
import { Modal, Button, Form } from "react-bootstrap";

const Smeta = ({ workId }) => {

  const [smetas, setSmetas] = useState([]);
  const [allDataLoad, setAllDataLoad] = useState(false);
  const [show, setShow] = useState(false);
  const [selectedSmeta, setSelectedSmeta] = useState(null);

  const fetchData = () => {
    setAllDataLoad(false);
    smetaService.getByWorkId(workId)
      .then(response => {
        setSmetas(response || []);
      })
      .catch(() => {
        toast.error('Ошибка при загрузке смет');
      })
      .finally(() => {
        setAllDataLoad(true);
      });
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleAdd = () => {
    setSelectedSmeta(null);
    setShow(true);
  };

  const handleEdit = (item) => {
    setSelectedSmeta(item);
    setShow(true);
  };

  const handleClose = () => {
    setShow(false);
    setSelectedSmeta(null);
  };

  const SmetaModal = ({ show, onClose, refresh, smeta }) => {

    const isEdit = !!smeta;

    const [materials, setMaterials] = useState([]);
    const [materialsForSelect, setMaterialsForSelect] = useState([]);
    const [materialDescription, setMaterialDescription] = useState("");
    const [materialCount, setMaterialCount] = useState(1);
    const [materialPrice, setMaterialPrice] = useState(0);
    const [materialId, setMaterialId] = useState(null);
    const [loading, setLoading] = useState(false);

    // загрузка материалов
    useEffect(() => {
      if (!show) return;

      setLoading(true);

      materialService.getAll()
        .then(response => {
          const data = response || [];
          setMaterials(data);

          setMaterialsForSelect(
            data.map(item => ({
              value: item.id,
              label: item.name
            }))
          );
        })
        .catch(() => toast.error("Ошибка при загрузке материалов"))
        .finally(() => setLoading(false));

    }, [show]);

    // если редактирование — заполняем форму
    useEffect(() => {
      if (!smeta) return;

      setMaterialId(smeta.material.id);
      setMaterialCount(smeta.count);
      setMaterialPrice(smeta.price);
      setMaterialDescription(smeta.material.description);

    }, [smeta]);

    const onMaterialChange = (selected) => {
      setMaterialId(selected.value);

      const material = materials.find(x => x.id === selected.value);
      if (material) {
        setMaterialDescription(material.description);
        setMaterialPrice(material.price);
      }
    };

    const deleteSmeta = () => {
      if (!smeta?.id) return;

      if (!window.confirm("Удалить смету?")) return;

      smetaService.removeSmetaById(smeta.id)
        .then(() => {
          refresh();
        })
        .catch(() => {
          toast.error("Ошибка при удалении");
        })
        .finally(() => {
          onClose();
        });
    };

    const saveSmeta = () => {

      const payload = {
        id: smeta?.id,
        workId: workId,
        materialId: materialId,
        count: materialCount,
        price: materialPrice
      };

      smetaService.addSmetaToWork(payload)
        .then(() => {
          refresh();
        })
        .catch(() => {
          toast.error("Ошибка при сохранении");
        })
        .finally(() => {
          onClose();
        });
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
                {isEdit ? "Редактировать смету" : "Добавить смету"}
              </Modal.Title>
            </Modal.Header>

            <Modal.Body>
              <Form>

                <Form.Group className="mb-3">
                  <Form.Label>Материал</Form.Label>
                  <Select
                    options={materialsForSelect}
                    value={materialsForSelect.find(x => x.value === materialId) || null}
                    onChange={onMaterialChange}
                    placeholder="Выберите материал"
                  />
                </Form.Group>

                <Form.Group className="mb-3">
                  <Form.Label>Описание</Form.Label>
                  <Form.Control value={materialDescription} disabled />
                </Form.Group>

                <Form.Group className="mb-3">
                  <Form.Label>Количество</Form.Label>
                  <Form.Control
                    type="number"
                    value={materialCount}
                    onChange={(e) => setMaterialCount(Number(e.target.value))}
                  />
                </Form.Group>

                <Form.Group>
                  <Form.Label>
                    Итого: {(materialPrice * materialCount).toLocaleString("ru-RU")} ₽
                  </Form.Label>
                </Form.Group>

              </Form>
            </Modal.Body>

            <Modal.Footer>
              {isEdit && (
                <Button variant="danger" onClick={deleteSmeta}>
                  Удалить
                </Button>
              )}
              <div className="ms-auto">
                <Button variant="primary" onClick={saveSmeta}>
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
        <BeatLoader color="#0d6efd" />
      </div>
    );
  }

  return (
    <div className='border rounded-4 shadow-sm p-3 bg-white'>
      <h5 className="mb-3">Смета:</h5>

      <div className="border rounded-4 shadow-sm" style={{ maxHeight: "120px", overflowY: "auto" }}>
        <div className="table-responsive">
          <table className="table table-sm table-hover align-middle mb-0">
            <thead className="table-light" style={{ position: "sticky", top: 0 }}>
              <tr>
                <th>№</th>
                <th>Название</th>
                <th>Описание</th>
                <th className="text-center">Кол-во</th>
                <th className="text-center">Цена</th>
                <th className="text-end">Итого</th>
              </tr>
            </thead>
            <tbody>
              {smetas.length > 0 ? (
                smetas
                  .sort((a, b) => a.id - b.id)
                  .map((item) => (
                    <tr
                      key={item.id}
                      style={{ cursor: "pointer" }}
                      onClick={() => handleEdit(item)}
                    >
                      <td>{item.id}</td>
                      <td className="fw-semibold">{item.material.name}</td>
                      <td className="text-muted small">{item.material.description}</td>
                      <td className="text-center">{item.count}</td>
                      <td className="text-center">
                        {item.price.toLocaleString("ru-RU")} ₽
                      </td>
                      <td className="text-end fw-semibold">
                        {(item.count * item.price).toLocaleString("ru-RU")} ₽
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

      <button type="button" className="btn btn-outline-primary btn-sm px-4 d-block mx-auto mt-3" onClick={handleAdd}>
        Добавить смету
      </button>

      <SmetaModal
        show={show}
        onClose={handleClose}
        refresh={fetchData}
        smeta={selectedSmeta}
      />
    </div>
  );
};

export default Smeta;