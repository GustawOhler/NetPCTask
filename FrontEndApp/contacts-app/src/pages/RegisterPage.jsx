import RegisterForm from "../components/RegisterForm";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();

  const handleRegister = async (username, email, password) => {
    try {
      await register({ userName: username, email, password });
      navigate("/");
    } catch {
      alert("Invalid credentials");
    }
  };

  return <RegisterForm onRegister={handleRegister} />;
}
