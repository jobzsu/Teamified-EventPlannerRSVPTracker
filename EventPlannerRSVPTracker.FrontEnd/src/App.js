import "./App.css";
import AppRoutes from "./utils/AppRoutes";
import { ToastContainer, Slide } from "react-toastify";

// function App() {
//   return (
//     <div className="App">
//       <header className="App-header">
//         <img src={logo} className="App-logo" alt="logo" />
//         <p>
//           Edit <code>src/App.js</code> and save to reload.
//         </p>
//         <a
//           className="App-link"
//           href="https://reactjs.org"
//           target="_blank"
//           rel="noopener noreferrer"
//         >
//           Learn React
//         </a>
//       </header>
//     </div>
//   );
// }

const App = () => {
  return (
    <>
      <AppRoutes />
      <ToastContainer
        position="top-right"
        transition={Slide}
        closeButton={false}
        hideProgressBar={true}
        theme="light"
      />
    </>
  );
};

export default App;
