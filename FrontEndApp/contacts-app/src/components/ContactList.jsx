export default function ContactList({ contacts, isLogged, onEdit, onDelete }) {
  return (
    <table>
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
      {contacts.map((c) => (
        <tr key={c.id}>
          <td>{c.firstName}</td>
          <td>{c.lastName}</td>
          <td>{c.email}</td>
          <td>{c.category}</td>
          <td>{c.subCategory}</td>
          <td>{c.telephoneNumber}</td>
          <td>{c.dateOfBirth}</td>
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
    </table>
  );
}
