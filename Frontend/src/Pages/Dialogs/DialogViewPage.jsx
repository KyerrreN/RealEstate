import { useAuth0 } from "@auth0/auth0-react";
import { useEffect, useState, useRef } from "react";
import { useParams } from "react-router-dom";
import axios from "axios";
import Header from "../../Components/Header/Header";
import Footer from "../../Components/Footer/Footer";
import { Button, TextField } from "@mui/material";
import SendIcon from "@mui/icons-material/Send";
import * as signalR from "@microsoft/signalr";

export default function DialogViewPage() {
    const { realEstateId, recieverId } = useParams();
    const { getAccessTokenSilently } = useAuth0();
    const [messages, setMessages] = useState([]);
    const [content, setContent] = useState("");
    const connectionRef = useRef(null);

    // load msgs
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

    // check messages, todelete
    useEffect(() => {
        console.log("MESSAGES");
        console.log(messages);
    }, [messages]);

    // connect to signalr
    useEffect(() => {
        const connectToHub = async () => {
            const token = await getAccessTokenSilently({
                authorizationParams: {
                    audience: "https://realestate.com/api",
                },
            });

            console.log("TOKEN: ", token);

            const connection = new signalR.HubConnectionBuilder()
                .withUrl("https://localhost:7055/chathub", {
                    accessTokenFactory: () => token,
                })
                .withAutomaticReconnect()
                .build();

            connection.on("RecieveMessage", (msg) => {
                setMessages((prev) => {
                    // если сообщение с таким id уже есть — не добавляем
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

        connectToHub();

        return () => {
            connectionRef.current?.stop();
        };
    }, [realEstateId, getAccessTokenSilently]);

    const handleSend = async () => {
        if (!content.trim()) return;

        try {
            await connectionRef.current?.invoke("SendMessage", {
                realEstateId,
                recieverId: recieverId,
                content,
            });

            setContent("");
        } catch (e) {
            console.error("Error sending a message: ", e);
        }
    };

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
                <TextField
                    id="outlined-basic"
                    variant="outlined"
                    sx={{ width: "100%" }}
                    value={content}
                    onChange={(e) => setContent(e.target.value)}
                    placeholder="Type your message..."
                    onKeyDown={(e) => {
                        if (e.key === "Enter") {
                            handleSend();
                        }
                    }}
                />
                <Button
                    variant="contained"
                    endIcon={<SendIcon />}
                    onClick={handleSend}
                >
                    Send
                </Button>
            </div>
            <Footer />
        </>
    );
}
