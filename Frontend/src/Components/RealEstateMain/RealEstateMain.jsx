import { useAuth0 } from "@auth0/auth0-react";
import "./RealEstateMain.css";

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
        <div
            className="container"
            style={{ display: "flex", justifyContent: "center" }}
        >
            {" "}
            {isAuthenticated ? (
                <span>
                    Hello. Your email: <strong>{user?.email}</strong>
                </span>
            ) : (
                <span>Please log in to use our system</span>
            )}
        </div>
    );
}
