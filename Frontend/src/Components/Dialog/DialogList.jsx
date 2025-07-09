import { List, ListItem, ListItemText } from "@mui/material";
import "./DialogList.css";

export default function DialogList({ dialogs }) {
    let renderData;

    if (dialogs.length == 0) {
        renderData = <p>You dont have any dialogs</p>;
    } else {
        renderData = dialogs.map((dialog) => {
            return (
                <ListItem key={dialog.realEstateId}>
                    <ListItemText
                        primary={dialog.realEstate.title}
                        secondary={dialog.realEstate.price}
                    />
                </ListItem>
            );
        });

        console.log(dialogs);
    }

    return (
        <div className="container">
            <h1>DIALOGS</h1>

            <List>{renderData}</List>
        </div>
    );
}
