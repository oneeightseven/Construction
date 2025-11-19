import MainTable from "./mainTable/mainTable";
import FilterTree from "./filters/filterTree";
import { useState } from 'react';
import Details from "./details/details";
import clients from '../images/clients.png';
import shoppingMalls from '../images/shoppingMalls.png';
import objects from '../images/objects.png';
import dogovor from '../images/dogovor.png';
import ClientsTable from "./tables/clientsTable";
import ConstructionObjectsTable from "./tables/constructionObjectsTable";
import ShoppingMallTable from "./tables/shoppingMallTable";
import ExcelExportForm from "./forms/excelExportForm";
import { useBase } from './contexts/BaseContext';
import "./mainPage.css"

const MainPage = ({ }) => {

  const [activeFilters, setActiveFilters] = useState([]);
  const [strictSearch, setStrictSearch] = useState(false);
  const [searchMode, setSearchMode] = useState('AND');
  const [selectedWork, setSelectedWork] = useState(null);

  const { currentTable, setCurrentTable } = useBase();

  const handleSearch = (filters, isStrict, mode) => {
    console.log('Получены фильтры:', filters, 'Строгий поиск:', isStrict, 'Режим:', mode);
    setActiveFilters(filters);
    setStrictSearch(isStrict);
    setSearchMode(mode);
  };

  const handleSelectWork = (work) => {
    setSelectedWork(work);
  }

  const resetSelectedWork = () => {
    setSelectedWork(null);
  }

  const setCurrentTableFunc = (tableName) => {
    setCurrentTable(tableName);
    setSelectedWork(null);
  } 

  return (
    <>
      <div>
        <div className="row header">
          <div className="col-1">
            <div onClick={() => setCurrentTableFunc("Clients")} className="icon-with-text">
              <img src={clients} />
              <span>Клиенты</span>
            </div>
          </div>
          <div className="col-1">
            <div onClick={() => setCurrentTableFunc("ShoppingMalls")}  className="icon-with-text">
              <img src={shoppingMalls} />
              <span>Торговые центры</span>
            </div>
          </div>
          <div className="col-1">
            <div onClick={() => setCurrentTableFunc("Objects")}  className="icon-with-text">
              <img src={objects} />
              <span>Объекты</span>
            </div>
          </div>
          <div className="col-1">
            <div onClick={() => setCurrentTableFunc("Details")} className="icon-with-text">
              <img src={dogovor} />
              <span>Детализация</span>
            </div>
          </div>
        </div>
      </div>
      <div className="container-fluid default-bg" style={{ marginTop: '20px' }}>
        {!currentTable ? (
          <div className="row">
          {!selectedWork ? (
            <>
              <div className="col-3">
                <FilterTree onSearch={handleSearch} />
              </div>
              <div className="col-6">
                <MainTable
                  filters={activeFilters}
                  strictSearch={strictSearch}
                  searchMode={searchMode}
                  setSelectedWork={handleSelectWork}
                />
              </div>
            </>
          ) : (<div className="col-6">
            <Details exitFunc={resetSelectedWork} work={selectedWork} />
          </div>
          )}

          <div className="col-3">
            {/* Дополнительная информация */}
          </div>
        </div>
        ):(
          <>
            {currentTable === "Objects" && <ConstructionObjectsTable/>}
            {currentTable === "Details" && <ExcelExportForm/>}
            {currentTable === "ShoppingMalls" && <ShoppingMallTable />}
            {currentTable === "Clients" && <ClientsTable/>}
          </>
        )}
        
      </div>
    </>

  );
};

export default MainPage;