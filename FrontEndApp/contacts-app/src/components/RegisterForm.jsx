import { useState } from "react";

export default function RegisterForm({ onRegister }) {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = (e) => {
    e.preventDefault();
    onRegister(username, email, password);
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Register new user</h2>
      <input placeholder="Username" value={username} onChange={(e) => setUsername(e.target.value)} required/>
      <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required/>
      <input type="password" placeholder="Password" value={password} onChange={(e) => setPassword(e.target.value)} required/>
      <button type="submit">Register</button>
    </form>
  );
};
