import React, { createContext, useContext, useState } from 'react';

const BaseContext = createContext();

export const BaseProvider = ({ children }) => {
  const [currentTable, setCurrentTable] = useState(null);

  return (
    <BaseContext.Provider value={{ currentTable, setCurrentTable }}>
      {children}
    </BaseContext.Provider>
  );
};

export const useBase = () => {
  const context = useContext(BaseContext);
  if (!context) {
    throw new Error('useBase must be used within a BaseProvider');
  }
  return context;
};