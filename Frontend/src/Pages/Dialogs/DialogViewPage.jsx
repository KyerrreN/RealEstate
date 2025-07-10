import { useAuth0 } from "@auth0/auth0-react";
import { useEffect, useState, useRef, useCallback } from "react";
import { useParams } from "react-router-dom";
import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import * as signalR from "@microsoft/signalr";
import Message from "../../Components/Message/Message";
import SendMessage from "../../Components/SendMessage/SendMessage";
import { getMessagesFromDialog } from "../../api/axiosChatClient";

export default function DialogViewPage() {
    const { realEstateId, recieverId } = useParams();
    const { getAccessTokenSilently } = useAuth0();
    const [messages, setMessages] = useState([]);
    const [signalRConnection, setSignalRConnection] = useState(null);

    const AUTH_AUDIENCE = import.meta.env.VITE_AUTH_AUDIENCE;
    const URL_CHATHUB = import.meta.env.VITE_URL_CHATHUB;

    const fetchMessages = useCallback(async () => {
        try {
            const token = await getAccessTokenSilently({
                authorizationParams: {
                    audience: AUTH_AUDIENCE,
                },
            });

            const data = await getMessagesFromDialog(token, realEstateId);

            setMessages(data);
        } catch (e) {
            console.error("Failed to fetch messages: " + e);
        }
    }, [getAccessTokenSilently, realEstateId]);

    const connectToHub = useCallback(async () => {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl(URL_CHATHUB, {
                accessTokenFactory: async () => {
                    return await getAccessTokenSilently({
                        authorizationParams: {
                            audience: AUTH_AUDIENCE,
                        },
                    });
                },
            })
            .withAutomaticReconnect()
            .build();

        connection.on("ReceiveMessage", (msg) => {
            setMessages((prev) => {
                if (prev.some((m) => m.id === msg.id)) {
                    return prev;
                }
                return [...prev, msg];
            });
        });

        await connection.start();
        await connection.invoke("JoinDialog", realEstateId);

        setSignalRConnection(connection);
    }, [getAccessTokenSilently, realEstateId]);

    useEffect(() => {
        fetchMessages();
    }, [realEstateId, getAccessTokenSilently]);

    useEffect(() => {
        connectToHub();

        return () => {
            signalRConnection?.stop();
        };
    }, [realEstateId, getAccessTokenSilently]);

    return (
        <>
            <Header />
            <div className="container">
                <h2>Messages for Real Estate: {realEstateId}</h2>
                <ul>
                    {messages.map((msg) => {
                        return <Message key={msg.id} message={msg} />;
                    })}
                </ul>
                <SendMessage
                    connection={signalRConnection}
                    realEstateId={realEstateId}
                    recieverId={recieverId}
                />
            </div>
            <Footer />
        </>
    );
}
