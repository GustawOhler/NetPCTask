import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import ContactDetails from "../components/ContactDetails";
import { getContactById } from "../api/contactsApi";
import { useAuth } from "../context/AuthContext";

// Page for showing details of a single contact
export default function ContactDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();
  const { isLogged } = useAuth();

  const [contact, setContact] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadContact = async () => {
      setIsLoading(true);
      setError(null);
      try {
        const data = await getContactById(id);
        setContact(data);
      } catch (err) {
        setError(err instanceof Error ? err.message : "Failed to load contact");
      } finally {
        setIsLoading(false);
      }
    };

    loadContact();
  }, [id]);

  if (isLoading) return <p>Loading contact...</p>;
  if (error) return <p>{error}</p>;
  if (!contact) return <p>Contact not found.</p>;

  return (
    <ContactDetails
      contact={contact}
      onEdit={isLogged ? () => navigate(`/contacts/${id}/edit`) : undefined}
      isLogged={isLogged}
    />
  );
}
