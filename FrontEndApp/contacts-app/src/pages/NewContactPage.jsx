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
  const [categories, setCategories] = useState([]);
  const [loadingCategories, setLoadingCategories] = useState(true);

  useEffect(() => {
    let ignore = false;

    const fetchCategories = async () => {
      try {
        const data = await getCategories();
        if (!ignore) setCategories(data);
      } catch (err) {
        if (!ignore) setErrorMessage("Failed to load categories");
      } finally {
        if (!ignore) setLoadingCategories(false);
      }
    };

    fetchCategories();
    return () => {
      ignore = true;
    };
  }, []);

  const handleSubmit = async (contact) => {
    try {
      await createContact(contact);
      navigate("/");
    } catch (err) {
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
      {loadingCategories ? <p>Loading categories...</p> : <ContactForm onSave={handleSubmit} categories={categories} />}
      <ErrorMessage message={errorMessage} />
    </div>
  );
}
