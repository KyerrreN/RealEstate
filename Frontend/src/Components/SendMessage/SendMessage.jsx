import { Button, TextField } from "@mui/material";
import SendIcon from "@mui/icons-material/Send";
import { useState } from "react";

export default function SendMessage({ connection, realEstateId, recieverId }) {
    const [content, setContent] = useState("");

    const handleSend = async () => {
        if (!content.trim()) return;

        try {
            await connection?.invoke("SendMessage", {
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
        </>
    );
}
