import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RealEstate from "./Pages/RealEstate/RealEstate";

function App() {
    return (
        <div className="wrapper">
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<RealEstate />} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;
