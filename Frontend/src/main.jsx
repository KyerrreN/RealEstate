import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import { Auth0Provider } from "@auth0/auth0-react";

createRoot(document.getElementById("root")).render(
    <StrictMode>
        <Auth0Provider
            domain="test-realestate.eu.auth0.com"
            clientId="JtQUUnXRkaA7E5LhhSQXwzMFUVEkIHBs"
            authorizationParams={{
                redirect_uri: window.location.origin,
                audience: "https://realestate.com/api",
            }}
        >
            <App />
        </Auth0Provider>
    </StrictMode>
);
