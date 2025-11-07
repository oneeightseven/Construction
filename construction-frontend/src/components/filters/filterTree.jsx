import { useState } from 'react';
import Select from 'react-select';

const FilterTree = ({ onSearch }) => {
    const [addingFilterMode, setAddingFilterMode] = useState(false);
    const [chosenFilters, setChosenFilters] = useState([]);
    const [currentId, setCurrentId] = useState(0);
    const [strictSearch, setStrictSearch] = useState(false);
    const [searchMode, setSearchMode] = useState('AND'); // 'AND' или 'OR'

    const dateFields = ['completionDate', 'dateBid', 'dateOfCreation', 'term'];
    
    const options = [
        { value: 'id', label: 'Номер' },
        { value: 'brand.name', label: 'Бренд' },
        { value: 'city.name', label: 'Город' },
        { value: 'completionDate', label: 'Дата завершения' },
        { value: 'constructionObject.name', label: 'Объект' },
        { value: 'dateBid', label: 'Дата заявки' },
        { value: 'dateOfCreation', label: 'Дата создания' },
        { value: 'shoppingMall.name', label: 'ТЦ' },
        { value: 'status.name', label: 'Статус' },
        { value: 'summ', label: 'Сумма ИТОГО' },
        { value: 'term', label: 'Срок' },
    ];

    const addFilter = (selected) => {
        if (!selected) return;
        
        const isDateField = dateFields.includes(selected.value);
        
        const filter = {
            id: currentId,
            value: selected.value,
            label: selected.label,
            isDateField: isDateField,
            inputValue: '',
            dateStart: '',
            dateEnd: ''
        };

        setChosenFilters(prevFilters => [...prevFilters, filter]);
        setCurrentId(prevId => prevId + 1);
        setAddingFilterMode(false);
    }

    const updateFilterValue = (filterId, value) => {
        setChosenFilters(prevFilters => 
            prevFilters.map(filter => 
                filter.id === filterId 
                    ? { ...filter, inputValue: value }
                    : filter
            )
        );
    }

    const updateDateValue = (filterId, field, value) => {
        setChosenFilters(prevFilters => 
            prevFilters.map(filter => 
                filter.id === filterId 
                    ? { ...filter, [field]: value }
                    : filter
            )
        );
    }

    const removeFilter = (filterId) => {
        setChosenFilters(prevFilters => 
            prevFilters.filter(filter => filter.id !== filterId)
        );
    }

    const handleSearch = () => {
        if (onSearch) {
            onSearch(chosenFilters, strictSearch, searchMode);
        }
    }

    const clearAllFilters = () => {
        setChosenFilters([]);
        if (onSearch) {
            onSearch([], strictSearch, searchMode);
        }
    }

    return (
        <div className="row">
            <div className="col-12">
                <div className="d-flex align-items-center mb-3">
                    <span className="fw-bold">Дерево фильтров</span>
                    <button 
                        onClick={() => setAddingFilterMode(true)} 
                        className="btn btn-outline-primary btn-sm" 
                        style={{marginLeft: '10px'}}
                        disabled={addingFilterMode}
                    >
                        + Добавить фильтр
                    </button>
                </div>

                {/* Настройки поиска */}
                <div className="mb-3 p-3 border rounded bg-light">
                    <div className="row g-3">
                        {/* Режим поиска И/ИЛИ */}
                        <div className="col-12">
                            <label className="form-label"><strong>Режим поиска:</strong></label>
                            <div className="btn-group w-100" role="group">
                                <button
                                    type="button"
                                    className={`btn btn-sm ${searchMode === 'AND' ? 'btn-primary' : 'btn-outline-primary'}`}
                                    onClick={() => setSearchMode('AND')}
                                >
                                    И (все условия)
                                </button>
                                <button
                                    type="button"
                                    className={`btn btn-sm ${searchMode === 'OR' ? 'btn-primary' : 'btn-outline-primary'}`}
                                    onClick={() => setSearchMode('OR')}
                                >
                                    ИЛИ (любое условие)
                                </button>
                            </div>
                            <small className="text-muted">
                                {searchMode === 'AND' 
                                    ? 'Все фильтры должны выполняться одновременно' 
                                    : 'Достаточно выполнения любого из фильтров'
                                }
                            </small>
                        </div>

                        {/* Строгий поиск */}
                        <div className="col-12">
                            <div className="form-check form-switch">
                                <input
                                    className="form-check-input"
                                    type="checkbox"
                                    id="strictSearch"
                                    checked={strictSearch}
                                    onChange={(e) => setStrictSearch(e.target.checked)}
                                />
                                <label className="form-check-label" htmlFor="strictSearch">
                                    <strong>Строгий поиск</strong>
                                    <small className="text-muted d-block">
                                        {strictSearch 
                                            ? 'Точное совпадение значений' 
                                            : 'Поиск по подстроке'
                                        }
                                    </small>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div className="filters-list">
                    {/* Существующие фильтры */}
                    {chosenFilters.map(filter => (
                        <div key={filter.id} className="filter-item p-3 border rounded mb-2 bg-light">
                            <div className="d-flex align-items-start">
                                <span className="fw-medium me-3 mt-1" style={{ minWidth: '120px' }}>
                                    {filter.label}
                                </span>
                                
                                <div className="flex-grow-1">
                                    {filter.isDateField ? (
                                        <div className="row g-2">
                                            <div className="col-sm-6">
                                                <input
                                                    type="date"
                                                    className="form-control form-control-sm"
                                                    placeholder="От"
                                                    value={filter.dateStart || ''}
                                                    onChange={(e) => updateDateValue(filter.id, 'dateStart', e.target.value)}
                                                />
                                            </div>
                                            <div className="col-sm-6">
                                                <input
                                                    type="date"
                                                    className="form-control form-control-sm"
                                                    placeholder="До"
                                                    value={filter.dateEnd || ''}
                                                    onChange={(e) => updateDateValue(filter.id, 'dateEnd', e.target.value)}
                                                />
                                            </div>
                                        </div>
                                    ) : (
                                        <input
                                            type="text"
                                            className="form-control form-control-sm"
                                            placeholder="Введите значение..."
                                            value={filter.inputValue || ''}
                                            onChange={(e) => updateFilterValue(filter.id, e.target.value)}
                                        />
                                    )}
                                </div>
                                
                                <button
                                    className="btn btn-outline-danger btn-sm ms-2"
                                    onClick={() => removeFilter(filter.id)}
                                    title="Удалить фильтр"
                                >
                                    ×
                                </button>
                            </div>
                        </div>
                    ))}
                    
                    {/* Новый фильтр - комбобокс в конце списка */}
                    {addingFilterMode && (
                        <div className="filter-item d-flex align-items-center p-2 border rounded border-primary bg-white">
                            <span className="fw-medium me-3" style={{ minWidth: '120px' }}>
                                Новый фильтр:
                            </span>
                            <div className="flex-grow-1 me-2">
                                <Select
                                    options={options}
                                    isSearchable={true}
                                    placeholder="Выберите параметр..."
                                    onChange={addFilter}
                                    autoFocus
                                    onBlur={() => setTimeout(() => setAddingFilterMode(false), 200)}
                                    menuPlacement="auto"
                                />
                            </div>
                            <button
                                className="btn btn-outline-secondary btn-sm"
                                onClick={() => setAddingFilterMode(false)}
                                title="Отмена"
                            >
                                ×
                            </button>
                        </div>
                    )}
                </div>

                {/* Кнопка выполнения поиска */}
                {chosenFilters.length > 0 && (
                    <div className="mt-4 p-3 border-top">
                        <div className="d-flex justify-content-between align-items-center">
                            <div>
                                <small className="text-muted">
                                    Выбрано фильтров: {chosenFilters.length}
                                    {strictSearch && <span className="text-warning ms-2">⚡ Строгий поиск</span>}
                                    <span className={`ms-2 ${searchMode === 'AND' ? 'text-info' : 'text-success'}`}>
                                        {searchMode === 'AND' ? '🔗 И' : '🔀 ИЛИ'}
                                    </span>
                                </small>
                            </div>
                            <div className="d-flex gap-2">
                                <button
                                    className="btn btn-outline-secondary btn-sm"
                                    onClick={clearAllFilters}
                                    title="Очистить все фильтры"
                                >
                                    Очистить все
                                </button>
                                <button
                                    className="btn btn-primary btn-sm"
                                    onClick={handleSearch}
                                >
                                    Выполнить поиск
                                </button>
                            </div>
                        </div>
                    </div>
                )}
            </div>
        </div>
    );
};

export default FilterTree;