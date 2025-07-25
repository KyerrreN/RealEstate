import { List, ListItem, ListItemButton, ListItemText } from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function DialogList({ dialogs }) {
    let renderData;
    const navigate = useNavigate();

    if (dialogs.length == 0) {
        renderData = <p>You dont have any dialogs</p>;
    } else {
        renderData = dialogs.map((dialog) => {
            return (
                <ListItem key={dialog.realEstateId}>
                    <ListItemButton
                        onClick={() =>
                            navigate(
                                `/dialogs/${dialog.realEstateId}/${dialog.interlocutorId}`
                            )
                        }
                    >
                        <ListItemText
                            primary={dialog.realEstate.title}
                            secondary={dialog.realEstate.price}
                        />
                    </ListItemButton>
                </ListItem>
            );
        });
    }

    return (
        <div className="container">
            <h1>DIALOGS</h1>

            <List>{renderData}</List>
        </div>
    );
}
