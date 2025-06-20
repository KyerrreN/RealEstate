import "./Header.css";
import Button from "@mui/material/Button";
import Divider from "@mui/material/Divider";

export default function Header() {
    return (
        <>
            <header className="container">
                <div>
                    <span>Real Estate UI</span>
                </div>

                <div>
                    <Button variant="contained" color="success">
                        Sign in
                    </Button>
                    <Button variant="contained">Sign up</Button>
                </div>
            </header>

            <Divider>Welcome</Divider>
        </>
    );
}
