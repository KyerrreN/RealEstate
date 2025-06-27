import { useAuth0 } from "@auth0/auth0-react";

export default function UserInfoBlock() {
    const { user } = useAuth0();

    return (
        <div className="container">
            <p>Username: {user.name}</p>
            <p>Email: {user.email}</p>
            <p>Phone number: </p>
            <p>First name</p>
            <p>Last name</p>
        </div>
    );
}
