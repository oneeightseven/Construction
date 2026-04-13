import { useState, useEffect } from 'react';
import '../details/details.css';
import 'react-toastify/dist/ReactToastify.css';
import { BeatLoader } from "react-spinners";



const Appendices = ({ }) => {

  const [appendices, setAppendices] = useState(null);
  const [allDataLoad, setAllDataLoad] = useState(false);

  const addTotals = () => {
    let objects = [];
    let moq1 = { id: 1, name: 'safetyPrecautions.docx', type: 'Документ', date: '11.04.2026' }
    let moq2 = { id: 2, name: 'layout1.png', type: 'Изображение', date: '10.04.2026' }
    let moq3 = { id: 3, name: 'layout2.png', type: 'Изображение', date: '10.04.2026' }
    let moq4 = { id: 4, name: 'calculations.xlsx', type: 'Таблица', date: '9.04.2026' }
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
            <h5 className="mb-3">Приложения к договору:</h5>
            <div className="border rounded-4 shadow-sm" style={{maxHeight: "120px", overflowY: "auto", backgroundColor: "#fff"}}>
              <div className="table-responsive">
                <table className="table table-sm table-hover align-middle mb-0">
                  <thead className="table-light" style={{position: "sticky", top: 0, zIndex: 1}}>
                    <tr>
                      <th>№</th>
                      <th className='text-center'>Название</th>
                      <th className='text-center'>Тип</th>
                      <th className='text-end'>Дата добавления</th>
                    </tr>
                  </thead>

                  <tbody>
                    {appendices?.length > 0 ? (
                      appendices.map((item, index) => (
                        <tr key={item.id || index}>
                          <td>{item.id}</td>
                          <td className="fw-semibold text-center">
                            {item.name}
                          </td>
                          <td className='text-center'>
                            {item.type}
                          </td>
                          <td className='text-end'>
                            {item.date}
                          </td>
                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td colSpan="4" className="text-center text-muted py-3">
                          Нет файлов
                        </td>
                      </tr>
                    )}
                  </tbody>
                </table>
              </div>
            </div>
            <button type="button" className="btn btn-outline-primary btn-sm px-4 d-block mx-auto mt-3" style={{ marginTop: '10px' }} onClick={() => (true)}>
              <i className="bi bi-check-all me-2"></i>Добавить файл
            </button>

          </div>
  )
};

export default Appendices;