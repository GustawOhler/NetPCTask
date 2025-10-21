import LoginForm from "../components/LoginForm";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleLogin = async (username, password) => {
    try {
      await login(username, password);
      navigate("/");
    } catch {
      alert("Invalid credentials");
    }
  };

  return (
    <div>
      <div>
        <p>Don't have an account?</p>
        <button onClick={() => navigate("/register")}>Register</button>
      </div>
      <LoginForm onLogin={handleLogin} />
    </div>
  );
}
