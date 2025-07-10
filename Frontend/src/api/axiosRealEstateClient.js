import axios from "axios";
const API_BASE_URL = import.meta.env.VITE_URL_REAL_ESTATE_API;

const api = axios.create({
    baseURL: API_BASE_URL,
    timeout: 10000,
    timeoutErrorMessage: "Couldn't connect to RealEstate API",
});

export const saveUser = async (firstName, lastName, phone, email, auth0Id) => {
    await api.post("users/", {
        firstName,
        lastName,
        phone,
        email,
        auth0Id,
    });
};
