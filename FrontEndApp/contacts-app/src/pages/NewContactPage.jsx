import { useEffect, useState } from "react";

import ContactForm from "../components/ContactForm";
import ErrorMessage from "../components/ErrorMessage";
import ValidationError from "../helpers/ValidationError";
import { createContact } from "../api/contactsApi";
import { getCategories } from "../api/categoryApi";
import { useNavigate } from "react-router-dom";

export default function NewContactPage() {
  const navigate = useNavigate();
  const [errorMessage, setErrorMessage] = useState("");
  const [availableCategories, setAvailableCategories] = useState([]);
  const [loadingCategories, setLoadingCategories] = useState(true);

  // Fetching available categories
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const data = await getCategories();
        setAvailableCategories(data);
      } catch (err) {
        setErrorMessage("Failed to load categories");
      } finally {
        setLoadingCategories(false);
      }
    };

    fetchCategories();
  }, []);

  const handleSubmit = async (contact) => {
    try {
      await createContact(contact);
      navigate("/");
    } catch (err) {
      // If validation error, use custom way of getting messages
      if (err instanceof ValidationError) {
        setErrorMessage(err.getMessages());
        return false;
      }
      setErrorMessage(err.message);
      return false;
    }
    return true;
  };

  return (
    <div>
      <h2>Create new contact</h2>
      {loadingCategories ? <p>Loading categories...</p> : <ContactForm onSave={handleSubmit} categories={availableCategories} />}
      <ErrorMessage message={errorMessage} />
    </div>
  );
}
