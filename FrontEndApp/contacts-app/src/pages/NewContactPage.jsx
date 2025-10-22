import ContactForm from "../components/ContactForm";
import ErrorMessage from "../components/ErrorMessage";
import ValidationError from "../helpers/ValidationError";
import { createContact } from "../api/contactsApi";
import { useNavigate } from "react-router-dom";
import { useState } from "react";

export default function NewContactPage() {
  const navigate = useNavigate();
  const [errorMessage, setErrorMessage] = useState("");

  const handleSubmit = async (contact) => {
    try {
      await createContact(contact);
      navigate("/");
    } catch (err) {
      if (err instanceof ValidationError) {
        setErrorMessage(err.getMessages());
        return;
      }
      setErrorMessage(err.message);
    }
  };

  return (
    <div>
      <h2>Create new contact</h2>
      <ContactForm onSave={handleSubmit} />
      <ErrorMessage message={errorMessage} />
    </div>
  );
}
