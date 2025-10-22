export default function ContactDetails({ contact, onEdit, isLogged }) {
  const formatDateOfBirth = () => {
    if (!contact?.dateOfBirth) return "-";
    const parsed = new Date(contact.dateOfBirth);
    if (Number.isNaN(parsed.getTime())) {
      const [fallback] = contact.dateOfBirth.split("T");
      return fallback || contact.dateOfBirth;
    }
    return parsed.toLocaleDateString();
  };

  return (
    <div>
      <h2>
        {contact.firstName} {contact.lastName}
      </h2>
      <dl>
        <dt>Email</dt>
        <dd>{contact.email}</dd>

        <dt>Category</dt>
        <dd>{contact.category?.visibleName ?? "-"}</dd>

        <dt>Subcategory</dt>
        <dd>{contact.subcategory?.visibleName ?? "-"}</dd>

        <dt>Telephone Number</dt>
        <dd>{contact.telephoneNumber ?? "-"}</dd>

        <dt>Date of Birth</dt>
        <dd>{formatDateOfBirth()}</dd>
      </dl>
      {isLogged && onEdit ? (
        <button type="button" onClick={onEdit}>
          Edit Contact
        </button>
      ) : null}
    </div>
  );
}
