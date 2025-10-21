import ContactForm from "../components/ContactForm";
import { createContact } from "../api/contactsApi";
import { useNavigate } from "react-router-dom";

export default function NewContactPage() {
  const navigate = useNavigate();

  const handleSubmit = (contact) => {
    createContact(contact);
    navigate("/");
  };

  return (
    <div>
      <h2>Create new contact</h2>
      <ContactForm onSave={handleSubmit} />
    </div>
  );
}
