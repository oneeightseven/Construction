import { useState, useEffect } from 'react';
import '../details/details.css';
import 'react-toastify/dist/ReactToastify.css';
import { BeatLoader } from "react-spinners";



const Accounts = ({ }) => {

  const [appendices, setAppendices] = useState(null);
  const [allDataLoad, setAllDataLoad] = useState(false);

  const addTotals = () => {
    let objects = [];
    let moq1 = { id: 1, appointment: 'Материалы', date: '11.04.2026', client: 'ООО МиниСтрой', sum: '5000' }
    let moq2 = { id: 2, appointment: 'Услуги', date: '10.04.2026', client: 'ООО МиниСтрой', sum: '15000'  }
    let moq3 = { id: 3, appointment: 'Услуги', date: '10.04.2026', client: 'ООО МиниСтрой', sum: '2600'  }
    let moq4 = { id: 4, appointment: 'Материалы', date: '9.04.2026', client: 'ООО МиниСтрой', sum: '33000'  }
    objects.push(moq1);
    objects.push(moq2);
    objects.push(moq3);
    objects.push(moq4);
    setAppendices(objects);
  }

  useEffect(() => {
    
    addTotals();
    setAllDataLoad(true);
  }, []);

  
  if (!allDataLoad) {
    return (
      <div className="d-flex justify-content-center align-items-center" style={{ height: "10vh" }}>
        <BeatLoader color="#0d6efd" size={15} />
      </div>
    );
  }


  return(
    <div className='border rounded-4 shadow-sm p-3 bg-white' style={{marginTop: '20px'}}>
            <h5 className="mb-3">Счета:</h5>
            <div className="border rounded-4 shadow-sm" style={{maxHeight: "120px", overflowY: "auto", backgroundColor: "#fff"}}>
              <div className="table-responsive">
                <table className="table table-sm table-hover align-middle mb-0">
                  <thead className="table-light" style={{position: "sticky", top: 0, zIndex: 1}}>
                    <tr>
                      <th>№</th>
                      <th className='text-center'>Назначение</th>
                      <th className='text-center'>Дата</th>
                      <th className='text-center'>Плательщик</th>
                      <th className='text-end'>Сумма</th>
                    </tr>
                  </thead>

                  <tbody>
                    {appendices?.length > 0 ? (
                      appendices.map((item, index) => (
                        <tr key={item.id || index}>
                          <td>{item.id}</td>
                          <td className="fw-semibold text-center">
                            {item.appointment}
                          </td>
                          <td className='text-center'>
                            {item.date}
                          </td>
                          <td className='text-center'>
                            {item.client}
                          </td>
                          <td className='text-end fw-semibold'>
                            {item.sum.toLocaleString("ru-RU")} ₽
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
            <button type="button" className="btn btn-outline-primary btn-sm px-4 d-block mx-auto mt-3" style={{ marginTop: '10px' }} onClick={() => (true)}>
              <i className="bi bi-check-all me-2"></i>Добавить счёт
            </button>

          </div>
  )
};

export default Accounts;