import { useAuth0 } from "@auth0/auth0-react";
import "./RealEstateMain.css";
import RealEstateCard from "../RealEstateCard/RealEstateCard";

export default function RealEstateMain() {
    const { isLoading, isAuthenticated, user } = useAuth0();
    if (isLoading) {
        return (
            <div
                className="container"
                style={{ display: "flex", justifyContent: "center" }}
            >
                <span>Loading your info...</span>
            </div>
        );
    }

    return (
        <div className="container real-estate-main ">
            {isAuthenticated ? (
                <span>
                    Hello. Your email: <strong>{user?.email}</strong>
                </span>
            ) : (
                <span>Please log in to use our system</span>
            )}
            <div className="real-estate-main-cards">
                {Array.from({ length: 10 }).map((_, index) => (
                    <RealEstateCard key={index} />
                ))}
            </div>
        </div>
    );
}
