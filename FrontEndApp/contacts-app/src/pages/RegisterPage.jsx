import ErrorMessage from "../components/ErrorMessage";
import RegisterForm from "../components/RegisterForm";
import ValidationError from "../helpers/ValidationError";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

export default function RegisterPage() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [errorMessages, setErrorMessages] = useState([]);

  const handleRegister = async (username, email, password) => {
    try {
      await register({ userName: username, email, password });
      navigate("/");
    } catch (e) {
      // If validation error, use custom way of getting messages
      if (e instanceof ValidationError) {
        setErrorMessages(e.getMessages());
        return;
      }
      setErrorMessages([e.message]);
    }
  };

  return (
    <div>
      <RegisterForm onRegister={handleRegister} />
      <ErrorMessage message={errorMessages} />
    </div>
  );
}
