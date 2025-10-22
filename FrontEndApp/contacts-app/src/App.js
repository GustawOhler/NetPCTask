import { BrowserRouter, Route, Routes, useLocation, useNavigate } from "react-router-dom";

import { AuthProvider } from "./context/AuthContext";
import ContactsPage from "./pages/ContactsPage";
import EditContactPage from "./pages/EditContactPage";
import LoginPage from "./pages/LoginPage";
import NewContactPage from "./pages/NewContactPage";
import PrivateRoute from "./routes/PrivateRoute";
import ContactDetailsPage from "./pages/ContactDetailsPage";
import RegisterPage from "./pages/RegisterPage";

function AppLayout() {
  const navigate = useNavigate();
  const location = useLocation();
  const canGoBack = typeof window !== "undefined" && window.history.length > 1;

  const handleBack = () => {
    if (canGoBack) {
      navigate(-1);
      return;
    }
    if (location.pathname !== "/") {
      navigate("/", { replace: true });
    }
  };

  return (
    <div>
      <button type="button" onClick={handleBack} disabled={!canGoBack && location.pathname === "/"}>
        Back
      </button>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        <Route path="/" element={<ContactsPage />} />
        <Route path="/contacts/:id" element={<ContactDetailsPage />} />
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
    </div>
  );
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <AppLayout />
      </BrowserRouter>
    </AuthProvider>
  );
}
