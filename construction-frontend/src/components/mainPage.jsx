import MainTable from "./mainTable/mainTable";
import FilterTree from "./filters/filterTree";
import { useState } from 'react';
import Details from "./details/details";

const MainPage = ({ }) => {

  const [activeFilters, setActiveFilters] = useState([]);
  const [strictSearch, setStrictSearch] = useState(false);
  const [searchMode, setSearchMode] = useState('AND');
  const [selectedWork, setSelectedWork] = useState(null);

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

  return (
    <div className="container-fluid" style={{ marginTop: '140px' }}>
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
            <Details exitFunc={resetSelectedWork} work={selectedWork}/>
          </div>
        )}

        <div className="col-3">
          {/* Дополнительная информация */}
        </div>
      </div>
    </div>
  );
};

export default MainPage;