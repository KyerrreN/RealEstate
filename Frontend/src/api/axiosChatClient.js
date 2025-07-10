import axios from "axios";
const API_BASE_URL = import.meta.env.VITE_URL_CHAT_API;

const api = axios.create({
    baseURL: API_BASE_URL,
    timeout: 5000,
    timeoutErrorMessage: `Couldn't connect to Chat API`,
});

export const getMessagesFromDialog = async (token, realEstateId) => {
    const response = await api.get(`/messages/realestate/${realEstateId}`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    return response.data;
};

export const getDialogs = async (token) => {
    const response = await api.get("/dialogs", {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    return response.data;
};
