import axios from "axios";

const api = axios.create({
    baseURL: "https://localhost:7055/api",
    timeout: 5000,
    timeoutErrorMessage: `Couldn't connect to Chat API`,
});

export const getMessagesFromDialog = async (token, realEstateId) => {
    const response = await api.get(
        `https://localhost:7055/api/messages/realestate/${realEstateId}`,
        {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        }
    );

    return response.data;
};

export const getDialogs = async (token) => {
    const response = await api.get("https://localhost:7055/api/dialogs", {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });

    return response.data;
};
