export default function ContactList({ contacts, isLogged, onEdit, onDelete, onView }) {
  const formatDateOfBirth = (value) => {
    if (!value) return "-";
    const parsed = new Date(value);
    // Fallback if value is not a date
    if (Number.isNaN(parsed.getTime())) {
      const [fallback] = value.split("T");
      return fallback || value;
    }
    return parsed.toLocaleDateString();
  };

  const handleRowClick = (id) => {
    if (onView) {
      onView(id);
    }
  };

  const handleRowKeyDown = (event, id) => {
    if (!onView) return;
    if (event.key === "Enter" || event.key === " ") {
      event.preventDefault();
      onView(id);
    }
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
          <tr
            key={c.id}
            onClick={() => handleRowClick(c.id)}
            onKeyDown={(event) => handleRowKeyDown(event, c.id)}
            role={onView ? "button" : undefined}
            tabIndex={onView ? 0 : undefined}
            style={{ cursor: onView ? "pointer" : "default" }}
          >
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
                  <button
                    type="button"
                    onClick={(event) => {
                      event.stopPropagation();
                      if (onEdit) onEdit(c.id);
                    }}
                  >
                    Edit
                  </button>
                  <button
                    type="button"
                    onClick={(event) => {
                      event.stopPropagation();
                      if (onDelete) onDelete(c.id);
                    }}
                  >
                    Delete
                  </button>
                </>
              ) : null}
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
