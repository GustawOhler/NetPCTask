import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

// Route only for logged user
export default function PrivateRoute({ children }) {
  const { isLogged } = useAuth();
  return isLogged ? children : <Navigate to="/" />;
}
