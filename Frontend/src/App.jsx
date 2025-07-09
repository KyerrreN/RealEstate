import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RealEstate from "./Pages/RealEstate/RealEstate";
import DialogsPage from "./Pages/Dialogs/DialogsPage";

function App() {
    return (
        <div className="wrapper">
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<RealEstate />} />
                    <Route path="/dialogs" element={<DialogsPage />} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;
