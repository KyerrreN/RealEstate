import "./RealEstateCard.css";
import PlaceholderImg from "../../utils/placeholder.jpg";
import Divider from "@mui/material/Divider";

export default function RealEstateCard() {
    return (
        <div className="card">
            <img src={PlaceholderImg} alt="Property" />

            <span className="card-header">Header PLACEHOLDER</span>
            <span className="card-desc">
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
                Reprehenderit quas iure eum libero nihil earum animi qui nulla,
                deserunt illum repudiandae tenetur sint inventore possimus
                cumque soluta, dolorem molestias blanditiis?
            </span>

            <Divider sx={{ mb: "10px" }} />

            <div className="card-meta">
                <span>Price: 120.00</span>
                <span>Address</span>
                <span>Property Type: PLACEHOLDER</span>
                <span>This property is available for PLACEHOLDER</span>
                <span>Owner: PLACEHOLDER</span>
                <span>City: PLACEHOLDER</span>
            </div>
        </div>
    );
}
