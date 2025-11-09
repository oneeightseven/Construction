import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import MainPage from './components/mainPage.jsx';
import { BaseProvider } from "./components/contexts/BaseContext.jsx";

function App() {

  return (
    <BaseProvider>
      <MainPage/>
    </BaseProvider>
  );
}

export default App;
