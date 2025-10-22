import { BASE_URL } from "../helpers/constants";

const API_URL = "/api/Categories";

export async function getCategories() {
  const res = await fetch(`${BASE_URL}${API_URL}`);
  if (!res.ok) throw new Error("Failed to fetch categories");
  return res.json();
}