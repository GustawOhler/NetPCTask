export default function ContactList({ contacts, isLogged, onEdit, onDelete }) {
  const formatDateOfBirth = (value) => {
    if (!value) return "-";
    const parsed = new Date(value);
    if (Number.isNaN(parsed.getTime())) {
      const [fallback] = value.split("T");
      return fallback || value;
    }
    return parsed.toLocaleDateString();
  };

  return (
    <table>
      <thead>
        <tr>
          <th>First Name</th>
          <th>Last Name</th>
          <th>Email</th>
          <th>Category</th>
          <th>Subcategory</th>
          <th>Telephone Number</th>
          <th>Date of Birth</th>
          <th />
        </tr>
      </thead>
      <tbody>
        {contacts.map((c) => (
          <tr key={c.id}>
            <td>{c.firstName}</td>
            <td>{c.lastName}</td>
            <td>{c.email}</td>
            <td>{c.category.visibleName}</td>
            <td>{c.subcategory?.visibleName ?? "-"}</td>
            <td>{c.telephoneNumber}</td>
            <td>{formatDateOfBirth(c.dateOfBirth)}</td>
            <td>
              {isLogged ? (
                <>
                  <button onClick={() => onEdit(c.id)}>Edit</button>
                  <button onClick={() => onDelete(c.id)}>❌</button>
                </>
              ) : null}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
