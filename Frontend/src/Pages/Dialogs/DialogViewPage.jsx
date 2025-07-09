import { useAuth0 } from "@auth0/auth0-react";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";

export default function DialogViewPage() {
    const { realEstateId } = useParams();
    const { getAccessTokenSilently } = useAuth0();
    const [messages, setMessages] = useState([]);

    useEffect(() => {
        const fetchMessages = async () => {
            try {
                const token = await getAccessTokenSilently({
                    authorizationParams: {
                        audience: "https://realestate.com/api",
                    },
                });

                const response = await axios.get(
                    `https://localhost:7055/api/messages/realestate/${realEstateId}`,
                    {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    }
                );

                setMessages(response.data);
            } catch (e) {
                console.error("Failed to fetch messages: " + e);
            }
        };

        fetchMessages();
    }, [realEstateId, getAccessTokenSilently]);

    useEffect(() => {
        console.log("MESSAGES");
        console.log(messages);
    }, [messages]);

    return (
        <>
            <Header />
            <div className="container">
                <h2>Dialogs for Real Estate: {realEstateId}</h2>
                <ul>
                    {messages.map((msg) => {
                        return <li key={msg.id}>Message: {msg.content}</li>;
                    })}
                </ul>
            </div>
            <Footer />
        </>
    );
}
