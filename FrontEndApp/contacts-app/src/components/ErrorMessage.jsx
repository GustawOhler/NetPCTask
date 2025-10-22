export default function ErrorMessage({ message }) {
  if (!message) return null;

  const messages = Array.isArray(message) ? message : [message];

  return (
    <div
      style={{
        border: "1px solid red",
        color: "red",
        backgroundColor: "#ffe6e6",
        padding: "8px",
        borderRadius: "4px",
        marginTop: "6px",
        fontSize: "0.9rem",
        whiteSpace: "pre-line", // preserves line breaks
      }}
    >
      {messages.map((msg, i) => (
        <div key={i}>{msg}</div>
      ))}
    </div>
  );
}
