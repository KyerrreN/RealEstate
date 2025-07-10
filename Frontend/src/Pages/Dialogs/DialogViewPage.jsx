import { useAuth0 } from "@auth0/auth0-react";
import { useEffect, useState, useRef } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import * as signalR from "@microsoft/signalr";
import Message from "../../Components/Message/Message";
import SendMessage from "../../Components/SendMessage/SendMessage";

export default function DialogViewPage() {
    const { realEstateId, recieverId } = useParams();
    const { getAccessTokenSilently } = useAuth0();
    const [messages, setMessages] = useState([]);
    const connectionRef = useRef(null);

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

    const connectToHub = async () => {
        const token = await getAccessTokenSilently({
            authorizationParams: {
                audience: "https://realestate.com/api",
            },
        });

        const connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7055/chathub", {
                accessTokenFactory: () => token,
            })
            .withAutomaticReconnect()
            .build();

        connection.on("RecieveMessage", (msg) => {
            setMessages((prev) => {
                if (prev.some((m) => m.id === msg.id)) {
                    return prev;
                }
                return [...prev, msg];
            });
        });

        await connection.start();
        await connection.invoke("JoinDialog", realEstateId);

        connectionRef.current = connection;
    };

    // load msgs
    useEffect(() => {
        fetchMessages();
    }, [realEstateId, getAccessTokenSilently]);

    useEffect(() => {
        connectToHub();

        return () => {
            connectionRef.current?.stop();
        };
    }, [realEstateId, getAccessTokenSilently]);

    return (
        <>
            <Header />
            <div className="container">
                <h2>Messages for Real Estate: {realEstateId}</h2>
                <ul>
                    {messages.map((msg) => {
                        return <Message message={msg} />;
                    })}
                </ul>
                <SendMessage
                    connection={connectionRef.current}
                    realEstateId={realEstateId}
                    recieverId={recieverId}
                />
            </div>
            <Footer />
        </>
    );
}
