import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import DialogList from "../../Components/Dialog/DialogList";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import { useCallback, useEffect, useState } from "react";
import { getDialogs } from "../../api/axiosChatClient";

export default function DialogsPage() {
    const { isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
    const navigate = useNavigate();
    const [dialogs, setDialogs] = useState([]);

    const fetchDialogs = useCallback(async () => {
        try {
            const token = await getAccessTokenSilently({
                authorizationParams: {
                    audience: "https://realestate.com/api",
                },
            });

            const data = await getDialogs(token);

            setDialogs(data);
        } catch (e) {
            console.error("Failed to fetch dialogs: ", e);
        }
    }, [getAccessTokenSilently]);

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            navigate("/");
        }
    }, [isLoading, isAuthenticated, navigate]);

    useEffect(() => {
        if (isAuthenticated) {
            fetchDialogs();
        }
    }, [isAuthenticated, fetchDialogs]);

    return (
        <>
            <Header />
            <DialogList dialogs={dialogs} />
            <Footer />
        </>
    );
}
