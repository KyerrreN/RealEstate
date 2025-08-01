import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import App from "./App.jsx";
import { Auth0Provider } from "@auth0/auth0-react";

const AUTH_DOMAIN = import.meta.env.VITE_AUTH_DOMAIN;
const AUTH_AUDIENCE = import.meta.env.VITE_AUTH_AUDIENCE;
const AUTH_CLIENT_ID = import.meta.env.VITE_AUTH_CLIENT_ID;

createRoot(document.getElementById("root")).render(
    <StrictMode>
        <Auth0Provider
            domain={AUTH_DOMAIN}
            clientId={AUTH_CLIENT_ID}
            authorizationParams={{
                redirect_uri: window.location.origin,
                audience: AUTH_AUDIENCE,
            }}
        >
            <App />
        </Auth0Provider>
    </StrictMode>
);
