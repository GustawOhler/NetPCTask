import { createContext, useContext, useState } from "react";
import { loginOnServer, refreshAccessToken, registerOnServer } from "../api/authApi";

const AuthContext = createContext();

export function AuthProvider({ children }) {
  const token = localStorage.getItem("token");

  const [isLogged, setIsLogged] = useState(token ? true : false);

  const login = async (username, password) => {
    const data = await loginOnServer(username, password);
    setIsLogged(true);
    localStorage.setItem("token", data.accessToken);
  };

  const refresh = async () => {
    const data = await refreshAccessToken();
    setIsLogged(true);
    localStorage.setItem("token", data.accessToken);
  };

  const register = async (user) => {
    await registerOnServer(user);
    await login(user.userName, user.password);
  };

  const logout = () => {
    setIsLogged(false);
    localStorage.removeItem("token");
  };

  return <AuthContext.Provider value={{ isLogged, login, refresh, register, logout }}>{children}</AuthContext.Provider>;
}

export const useAuth = () => useContext(AuthContext);
