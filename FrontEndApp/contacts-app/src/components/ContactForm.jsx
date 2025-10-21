import { useEffect, useState } from "react";

export default function ContactForm({ onSave, initialData }) {
  const [id, setId] = useState(null);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [category, setCategory] = useState("");
  const [subCategory, setSubCategory] = useState("");
  const [telephoneNumber, setTelephoneNumber] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState(new Date());

  useEffect(() => {
    if (initialData) {
      setId(initialData.id || null);
      setFirstName(initialData.firstName || "");
      setLastName(initialData.lastName || "");
      setEmail(initialData.email || "");
      setCategory(initialData.category || "");
      setSubCategory(initialData.subCategory || "");
      setTelephoneNumber(initialData.telephoneNumber || "");
      setDateOfBirth(initialData.dateOfBirth?.split("T")[0] || "");
    }
  }, [initialData]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await onSave({ id, firstName, lastName, email, category, subCategory, telephoneNumber, dateOfBirth });
    setId(null);
    setFirstName("");
    setLastName("");
    setEmail("");
    setCategory("");
    setSubCategory("");
    setTelephoneNumber("");
    setDateOfBirth("");
  };

  return (
    <form onSubmit={handleSubmit}>
      <input placeholder="First Name" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
      <input placeholder="Last Name" value={lastName} onChange={(e) => setLastName(e.target.value)} />
      <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} />
      <input
        placeholder="Telephone Number"
        value={telephoneNumber}
        onChange={(e) => setTelephoneNumber(e.target.value)}
      />
      <label>
        Date of Birth:
        <input type="date" value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} />
      </label>

      <label>
        Category:
        <select value={category} onChange={(e) => setCategory(e.target.value)}>
          <option value="">-- select category --</option>
          <option value="Private">Private</option>
          <option value="Business">Business</option>
          <option value="Other">Other</option>
        </select>
      </label>

      <label>
        Subcategory:
        {category === "Business" ? (
          <select value={subCategory} onChange={(e) => setSubCategory(e.target.value)}>
            <option value="">-- select subcategory --</option>
            <option value="Boss">Boss</option>
            <option value="Client">Client</option>
            <option value="Coworker">Coworker</option>
          </select>
        ) : (
          <input placeholder="Subcategory" value={subCategory} onChange={(e) => setSubCategory(e.target.value)} />
        )}
      </label>

      <button type="submit">Add</button>
    </form>
  );
}
