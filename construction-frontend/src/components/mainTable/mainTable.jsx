import { workService } from '../../services/workService.js';
import '../mainTable/mainTable.css';
import { useState, useEffect } from 'react';

const MainTable = ({ filters = [], strictSearch = false, searchMode = 'AND', setSelectedWork }) => {
    const [allWorks, setAllWorks] = useState([]);
    const [filteredWorks, setFilteredWorks] = useState([]);
    const [chosenWork, setChosenWork] = useState();
    const [loading, setLoading] = useState(false);

    const workFields = ['id', 'dateBid', 'term', 'completionDate', 'city.name',
        'shoppingMall.name', 'brand.name', 'status.name',
        'constructionObject.name', 'client.name', 'dateOfCreation', 'summ'];

    useEffect(() => {
        loadWorks();
    }, []);

    useEffect(() => {
        if (allWorks.length > 0) {
            applyFilters(filters, strictSearch, searchMode);
        }
    }, [filters, strictSearch, searchMode, allWorks]);

    const choseWork = (work) => {
        setChosenWork(work);
    }

    const handleDoubleClick = (work) => {
        setSelectedWork(work);
    };

    const loadWorks = async () => {
        try {
            setLoading(true);
            const data = await workService.getAll();
            setAllWorks(data);
            setFilteredWorks(data);
            console.log('Загружены все работы:', data);
        } catch (error) {
            console.error('Ошибка загрузки:', error);
        } finally {
            setLoading(false);
        }
    };

    const applyFilters = (currentFilters, isStrict, mode) => {
        if (!currentFilters || currentFilters.length === 0) {
            setFilteredWorks(allWorks);
            return;
        }

        const filtered = allWorks.filter(work => {
            if (mode === 'AND') {
                // Режим И - все фильтры должны выполняться
                return currentFilters.every(filter =>
                    checkFilter(work, filter, isStrict)
                );
            } else {
                // Режим ИЛИ - хотя бы один фильтр должен выполняться
                return currentFilters.some(filter =>
                    checkFilter(work, filter, isStrict)
                );
            }
        });

        setFilteredWorks(filtered);
        console.log(`Отфильтровано: ${filtered.length} из ${allWorks.length} работ (режим: ${mode}, строгий: ${isStrict})`);
    };

    const checkFilter = (work, filter, isStrict) => {
        const workValue = getFieldValue(work, filter.value);

        if (filter.isDateField) {
            return filterByDateRange(workValue, filter);
        } else {
            return isStrict
                ? filterByStrictText(workValue, filter)
                : filterByTextField(workValue, filter);
        }
    };

    const filterByDateRange = (workValue, filter) => {
        if (!workValue) return false;

        const workDate = new Date(workValue);
        const startDate = filter.dateStart ? new Date(filter.dateStart) : null;
        const endDate = filter.dateEnd ? new Date(filter.dateEnd) : null;

        if (!startDate && !endDate) return true;

        if (startDate && endDate) {
            return workDate >= startDate && workDate <= endDate;
        } else if (startDate) {
            return workDate >= startDate;
        } else if (endDate) {
            return workDate <= endDate;
        }

        return true;
    };

    const filterByTextField = (workValue, filter) => {
        if (!filter.inputValue) return true;
        if (!workValue) return false;
        return workValue.toString().toLowerCase().includes(filter.inputValue.toLowerCase());
    };

    const filterByStrictText = (workValue, filter) => {
        if (!filter.inputValue) return true;
        if (!workValue) return false;
        return workValue.toString().toLowerCase() === filter.inputValue.toLowerCase();
    };

    const getFieldValue = (work, fieldPath) => {
        if (fieldPath.includes('.')) {
            return fieldPath.split('.').reduce((obj, key) => obj?.[key], work);
        }
        return work[fieldPath];
    };

    const getAlertType = () => {
        if (searchMode === 'OR') return 'alert-success';
        return strictSearch ? 'alert-warning' : 'alert-info';
    };

    const getModeDescription = () => {
        if (searchMode === 'AND') {
            return strictSearch ? '⚡ Строгий поиск (все условия)' : '🔗 Все условия';
        } else {
            return strictSearch ? '⚡ Строгий поиск (любое условие)' : '🔀 Любое условие';
        }
    };

    const headers = ["Номер", "Дата заявки", "Срок", "Дата завершения", "Город", "ТЦ", "Бренд", "Статус", "Объект", "Клиент", "Дата создания", "Сумма ИТОГО"];

    return (
    <div style={{ height: '1200px', overflow: 'auto', fontSize: '12px' }} className="p-2">

        {filters.length > 0 && (
            <div className={`alert ${getAlertType()} mb-2 py-2`}>
                <small>
                    Найдено работ: {filteredWorks.length} из {allWorks.length}
                    {filters.length > 0 && ` • Активных фильтров: ${filters.length}`}
                    {` • ${getModeDescription()}`}
                </small>
            </div>
        )}

        {loading ? (
            <div className="text-center p-4">
                <div className="spinner-border text-primary" role="status">
                    <span className="visually-hidden">Загрузка...</span>
                </div>
            </div>
        ) : (
            <div className="table-responsive">
                <table
                    className="table table-hover table-bordered align-middle"
                    style={{ tableLayout: 'fixed', width: '100%' }}
                >
                    <thead className="table-light sticky-top">
                        <tr>
                            {headers.map((header, index) => (
                                <th key={index}>{header}</th>
                            ))}
                        </tr>
                    </thead>

                    <tbody>
                        {filteredWorks.map((work) => (
                            <tr
                                key={work.id}
                                onDoubleClick={() => handleDoubleClick(work)}
                                onClick={() => choseWork(work)}
                                className={
                                    chosenWork?.id === work.id
                                        ? "table-primary"
                                        : ""
                                }
                                style={{ cursor: "pointer" }}
                            >
                                {workFields.map((field) => {
                                    const value = getFieldValue(work, field);

                                    return (
                                        <td
                                            key={field}
                                            title={value}
                                            style={{
                                                whiteSpace: 'nowrap',
                                                overflow: 'hidden',
                                                textOverflow: 'ellipsis'
                                            }}
                                        >
                                            {value}
                                        </td>
                                    );
                                })}
                            </tr>
                        ))}

                        {filteredWorks.length === 0 && (
                            <tr>
                                <td
                                    colSpan={headers.length}
                                    className="text-center text-muted p-4"
                                >
                                    {allWorks.length === 0
                                        ? "Нет данных"
                                        : "По заданным фильтрам ничего не найдено"}
                                </td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </div>
        )}
    </div>
);
};

export default MainTable;