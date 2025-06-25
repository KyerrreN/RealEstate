import { useAuth0 } from "@auth0/auth0-react";
import "./RealEstateMain.css";
import RealEstateCard from "../RealEstateCard/RealEstateCard";
import { useMemo } from "react";

export default function RealEstateMain() {
    // TEMP
    const cards = useMemo(
        () =>
            Array.from({ length: 10 }).map(() => ({ id: crypto.randomUUID() })),
        []
    );

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
                {cards.map((card) => (
                    <RealEstateCard key={card.id} />
                ))}
            </div>
        </div>
    );
}
