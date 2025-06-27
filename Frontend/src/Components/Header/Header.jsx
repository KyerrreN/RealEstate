import { useAuth0 } from "@auth0/auth0-react";
import "./Header.css";
import Button from "@mui/material/Button";
import { useEffect } from "react";
import axios from "axios";

export default function Header() {
    const { loginWithRedirect, logout, isAuthenticated, isLoading, user } =
        useAuth0();

    let authButton;

    useEffect(() => {
        const sendUserToBackend = async () => {
            if (!user || !isAuthenticated) return;

            const { isSignedUp } = user;

            if (isSignedUp !== undefined) {
                const { firstName, lastName, phoneNumber } = user;

                try {
                    await axios.post("https://localhost:7283/api/users/", {
                        firstName,
                        lastName,
                        phone: phoneNumber,
                        email: user.email,
                        auth0Id: user.sub,
                    });
                } catch (e) {
                    console.error(e);
                }
            }
        };

        sendUserToBackend();
    }, [user, isAuthenticated]);

    if (isLoading) {
        authButton = <Button variant="outlined">Please wait</Button>;
    } else if (isAuthenticated) {
        authButton = (
            <Button
                variant="outlined"
                color="error"
                onClick={() =>
                    logout({
                        logoutParams: {
                            returnTo: window.location.origin,
                        },
                    })
                }
            >
                Logout
            </Button>
        );
    } else {
        authButton = (
            <>
                <Button
                    variant="contained"
                    color="success"
                    onClick={() => loginWithRedirect()}
                >
                    Sign in
                </Button>
                <Button
                    variant="contained"
                    onClick={() =>
                        loginWithRedirect({
                            authorizationParams: {
                                screen_hint: "signup",
                            },
                        })
                    }
                >
                    Sign up
                </Button>
            </>
        );
    }

    return (
        <header className="container">
            <div>
                <span>Real Estate UI</span>
            </div>
            <div>{authButton}</div>
        </header>
    );
}
