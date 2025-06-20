import { useAuth0 } from "@auth0/auth0-react";
import "./Header.css";
import Button from "@mui/material/Button";
import Divider from "@mui/material/Divider";

export default function Header() {
    const { loginWithRedirect, logout, isAuthenticated, user, isLoading } =
        useAuth0();

    return (
        <>
            <header className="container">
                <div>
                    <span>Real Estate UI</span>
                </div>

                <div>
                    {isLoading ? (
                        <Button variant="outlined">Please wait</Button>
                    ) : isAuthenticated ? (
                        <>
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
                        </>
                    ) : (
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
                    )}
                </div>
            </header>

            <Divider>Welcome</Divider>
        </>
    );
}
