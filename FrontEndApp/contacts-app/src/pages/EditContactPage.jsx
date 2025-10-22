import { getContactById, updateContact } from "../api/contactsApi";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import ContactForm from "../components/ContactForm";
import ErrorMessage from "../components/ErrorMessage";
import ValidationError from "../helpers/ValidationError";
import { getCategories } from "../api/categoryApi";

// Page for editing single contact
export default function EditContactPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [contact, setContact] = useState(null);
  const [availableCategories, setAvailableCategories] = useState([]);
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState(null);

  // Fetching contact info and available categories
  useEffect(() => {
    const fetchData = async () => {
      try {
        const [contactData, categoriesData] = await Promise.all([getContactById(id), getCategories()]);
        setContact(contactData);
        setAvailableCategories(categoriesData);
      } catch (err) {
        setErrorMessage(err instanceof Error ? err.message : "Failed to load data");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [id]);

  const handleSubmit = async (updated) => {
    setErrorMessage(null);
    try {
      await updateContact(id, updated);
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

  if (loading) return <p>Loading...</p>;
  if (!contact) return <p>Contact not found.</p>;

  return (
    <div>
      <h2>Edit Contact</h2>
      <ContactForm onSave={handleSubmit} initialData={contact} categories={availableCategories} />
      <ErrorMessage message={errorMessage} />
    </div>
  );
}
