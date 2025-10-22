import { getContactById, updateContact } from "../api/contactsApi";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import ContactForm from "../components/ContactForm";
import ErrorMessage from "../components/ErrorMessage";
import ValidationError from "../helpers/ValidationError";

export default function EditContactPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [contact, setContact] = useState(null);
  const [loading, setLoading] = useState(true);
  const [errorMessage, setErrorMessage] = useState([]);

  useEffect(() => {
    const fetchContact = async () => {
      try {
        const data = await getContactById(id);
        setContact(data);
      } catch (err) {
        setErrorMessage("Failed to load contact");
      } finally {
        setLoading(false);
      }
    };
    fetchContact();
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
      <ContactForm onSave={handleSubmit} initialData={contact} />
      <ErrorMessage message={errorMessage} />
    </div>
  );
}
