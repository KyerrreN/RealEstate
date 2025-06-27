import "./App.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import RealEstate from "./Pages/RealEstate/RealEstate";
import UserPage from "./Pages/User/UserPage";

function App() {
    return (
        <div className="wrapper">
            <BrowserRouter>
                <Routes>
                    <Route path="/" element={<RealEstate />} />
                    <Route path="/user" element={<UserPage />} />
                </Routes>
            </BrowserRouter>
        </div>
    );
}

export default App;
