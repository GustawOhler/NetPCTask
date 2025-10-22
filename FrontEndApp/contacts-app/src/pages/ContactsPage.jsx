import { deleteContact, getContacts } from "../api/contactsApi";
import { useEffect, useState } from "react";

import ContactList from "../components/ContactList";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

// Page for showing list of contacts
export default function ContactsPage() {
  const [contacts, setContacts] = useState([]);
  const { isLogged, logout } = useAuth();
  const navigate = useNavigate();

  const loadContacts = async () => setContacts(await getContacts());

  useEffect(() => {
    loadContacts();
  }, []);

  const handleDelete = async (id) => {
    await deleteContact(id);
    loadContacts();
  };

  return (
    <div>
      <h2>Contacts</h2>
      {isLogged ? <button onClick={logout}>Logout</button> : <button onClick={() => navigate("/login")}>Login</button>}
      {isLogged ? <button onClick={() => navigate("/contacts/create")}>Create new contact</button> : null}
      <ContactList
        contacts={contacts}
        onDelete={handleDelete}
        onEdit={(id) => navigate(`/contacts/${id}/edit`)}
        onView={(id) => navigate(`/contacts/${id}`)}
        isLogged={isLogged}
      />
    </div>
  );
}
