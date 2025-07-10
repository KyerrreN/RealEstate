export default function Message({ message }) {
    return <li key={message.id}>Message: {message.content}</li>;
}
