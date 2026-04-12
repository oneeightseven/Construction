import { useState, useEffect } from 'react';
import '../details/details.css';
import 'react-toastify/dist/ReactToastify.css';
import { BeatLoader } from "react-spinners";



const Estimate = ({ }) => {

  const [estimates, setEstimates] = useState(null);
  const [allDataLoad, setAllDataLoad] = useState(false);

  const addTotals = () => {
    let objects = [];
    let moq1 = { id: 1, name: 'Ламинат (1х1)', description: 'Черный', price: 1000, quantity: 5 }
    let moq2 = { id: 2, name: 'Обои (2x2)', description: 'Белые', price: 1500, quantity: 10 }
    let moq3 = { id: 3, name: 'Гвозди (100гм)', description: 'Б/У', price: 150, quantity: 130 }
    let moq4 = { id: 4, name: 'Клей (1л)', description: 'Розовый', price: 300, quantity: 3 }
    objects.push(moq1);
    objects.push(moq2);
    objects.push(moq3);
    objects.push(moq4);
    setEstimates(objects);
  }

  useEffect(() => {
    
    addTotals();
    setAllDataLoad(true);
  }, []);

  
  if (!allDataLoad) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "60vh" }}>
        <BeatLoader color="#0d6efd" size={15} />
      </div>
    );
  }


  return(
    <div className='border rounded-4 shadow-sm p-3 bg-white'>
            <h5 className="mb-3">Смета:</h5>
            <div className="border rounded-4 shadow-sm" style={{maxHeight: "120px", overflowY: "auto", backgroundColor: "#fff"}}>
              <div className="table-responsive">
                <table className="table table-sm table-hover align-middle mb-0">
                  <thead className="table-light" style={{position: "sticky", top: 0, zIndex: 1}}>
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
                    {estimates?.length > 0 ? (
                      estimates.map((item, index) => (
                        <tr key={item.id || index}>
                          <td>{item.id}</td>
                          <td className="fw-semibold">
                            {item.name}
                          </td>
                          <td className="text-muted small">
                            {item.description || "—"}
                          </td>
                          <td className="text-center">
                            {item.quantity}
                          </td>
                          <td className="text-center">
                            {item.price?.toLocaleString("ru-RU")} ₽
                          </td>
                          <td className="text-end fw-semibold">
                            {(item.quantity * item.price)?.toLocaleString("ru-RU")} ₽
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
            <button type="button" className="btn btn-outline-primary btn-sm px-4 d-block mx-auto mt-3" style={{ marginTop: '10px' }} onClick={() => (true)}>
              <i className="bi bi-check-all me-2"></i>Добавить смету
            </button>

          </div>
  )
};

export default Estimate;