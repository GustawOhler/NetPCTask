import { useEffect, useMemo, useState } from "react";

export default function ContactForm({ onSave, initialData, categories = [] }) {
  const [id, setId] = useState(null);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [category, setCategory] = useState("");
  const [subCategory, setSubCategory] = useState("");
  const [telephoneNumber, setTelephoneNumber] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState(new Date());

  const selectedCategory = useMemo(() => categories.find((c) => c.name === category), [categories, category]);
  const isBusinessCategory = selectedCategory?.name?.toLowerCase() === "business";
  const subcategoryOptions = selectedCategory?.subcategories ?? [];
  const subcategoryDataListId = selectedCategory ? `subcategory-options-${selectedCategory.id}` : "subcategory-options";

  useEffect(() => {
    if (initialData) {
      setId(initialData.id || null);
      setFirstName(initialData.firstName || "");
      setLastName(initialData.lastName || "");
      setEmail(initialData.email || "");
      setCategory(initialData.category.name || "");
      setSubCategory(initialData.subCategory?.name || "");
      setTelephoneNumber(initialData.telephoneNumber || "");
      setDateOfBirth(initialData.dateOfBirth?.split("T")[0] || "");
    }
  }, [initialData]);

  const handleCategoryChange = (e) => {
    setCategory(e.target.value);
    setSubCategory("");
  };

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
      <input placeholder="First Name" value={firstName} onChange={(e) => setFirstName(e.target.value)} required />
      <input placeholder="Last Name" value={lastName} onChange={(e) => setLastName(e.target.value)} required />
      <input placeholder="Email" value={email} onChange={(e) => setEmail(e.target.value)} required />
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
        <select value={category} onChange={handleCategoryChange} required>
          <option value="">-- select category --</option>
          {categories.map((c) => (
            <option key={c.id ?? c.name} value={c.name}>
              {c.visibleName}
            </option>
          ))}
        </select>
      </label>

      <label>
        Subcategory:
        {isBusinessCategory ? (
          <select value={subCategory} onChange={(e) => setSubCategory(e.target.value)} required>
            <option value="">-- select subcategory --</option>
            {subcategoryOptions.map((sub) => (
              <option key={sub.id ?? sub.name} value={sub.name}>
                {sub.visibleName}
              </option>
            ))}
          </select>
        ) : (
          <>
            <input
              list={subcategoryOptions.length ? subcategoryDataListId : undefined}
              placeholder="Subcategory"
              value={subCategory}
              onChange={(e) => setSubCategory(e.target.value)}
            />
            {subcategoryOptions.length ? (
              <datalist id={subcategoryDataListId}>
                {subcategoryOptions.map((sub) => (
                  <option key={sub.id ?? sub.name} value={sub.name}>
                    {sub.visibleName}
                  </option>
                ))}
              </datalist>
            ) : null}
          </>
        )}
      </label>

      <button type="submit">Add</button>
    </form>
  );
}
