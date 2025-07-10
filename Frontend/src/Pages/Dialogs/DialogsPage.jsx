import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import DialogList from "../../Components/Dialog/DialogList";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";

export default function DialogsPage() {
    const { isAuthenticated, isLoading, getAccessTokenSilently } = useAuth0();
    const navigate = useNavigate();
    const [dialogs, setDialogs] = useState([]);

    useEffect(() => {
        if (!isLoading && !isAuthenticated) {
            navigate("/");
        }
    }, [isLoading, isAuthenticated, navigate]);

    useEffect(() => {
        const fetchDialogs = async () => {
            try {
                const token = await getAccessTokenSilently({
                    authorizationParams: {
                        audience: "https://realestate.com/api",
                    },
                });

                const response = await axios.get(
                    "https://localhost:7055/api/dialogs",
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                setDialogs(response.data);
            } catch (e) {
                console.error("Failed to fetch dialogs: ", e);
            }
        };

        if (isAuthenticated) {
            fetchDialogs();
        }
    }, [isAuthenticated, getAccessTokenSilently]);

    return (
        <>
            <Header />
            <DialogList dialogs={dialogs} />
            <Footer />
        </>
    );
}
