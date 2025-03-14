import logo from './logo.svg';
import './App.css';
import MultiSelect from './Multiselect/multiSelect';

function App() {
  return (
    <div className="App">
      {/* <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>
      <div>
      <input id='fileUpload' type='file' multiple
        accept='application/pdf, image/png'/>
      </div> */}
      <MultiSelect></MultiSelect>
    </div>
  );
}

export default App;
