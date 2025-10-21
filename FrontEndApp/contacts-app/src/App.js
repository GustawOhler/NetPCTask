import { BrowserRouter, Route, Routes } from "react-router-dom";

import { AuthProvider } from "./context/AuthContext";
import ContactsPage from "./pages/ContactsPage";
import EditContactPage from "./pages/EditContactPage";
import LoginPage from "./pages/LoginPage";
import NewContactPage from "./pages/NewContactPage";
import PrivateRoute from "./routes/PrivateRoute";
import RegisterPage from "./pages/RegisterPage";

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/" element={<ContactsPage />} />
          <Route
            path="/contacts/create"
            element={
              <PrivateRoute>
                <NewContactPage />
              </PrivateRoute>
            }
          />
          <Route
            path="/contacts/:id/edit"
            element={
              <PrivateRoute>
                <EditContactPage />
              </PrivateRoute>
            }
          />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}
