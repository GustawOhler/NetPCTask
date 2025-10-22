import { getContactById, updateContact } from "../api/contactsApi";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import ContactForm from "../components/ContactForm";
import ErrorMessage from "../components/ErrorMessage";
import ValidationError from "../helpers/ValidationError";
import { getCategories } from "../api/categoryApi";

export default function EditContactPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [contact, setContact] = useState(null);
  const [categories, setCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState([]);

  useEffect(() => {
    let ignore = false;

    const fetchData = async () => {
      try {
        const [contactData, categoriesData] = await Promise.all([getContactById(id), getCategories()]);
        if (ignore) return;
        setContact(contactData);
        setCategories(categoriesData);
      } catch (err) {
        if (ignore) return;
        setErrorMessage(err instanceof Error ? err.message : "Failed to load data");
      } finally {
        if (!ignore) setLoading(false);
      }
    };

    fetchData();
    return () => {
      ignore = true;
    };
  }, [id]);

  const handleSubmit = async (updated) => {
    try {
      await updateContact(id, updated);
      navigate("/");
    } catch (err) {
      if (err instanceof ValidationError) {
        setErrorMessage(err.getMessages());
        return;
      }
      setErrorMessage(err.message);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (!contact) return <p>Contact not found.</p>;

  return (
    <div>
      <h2>Edit Contact</h2>
      <ContactForm onSave={handleSubmit} initialData={contact} categories={categories} />
      <ErrorMessage message={errorMessage} />
    </div>
  );
}
