import { BASE_URL } from "../helpers/constants";
import ValidationError from "../helpers/ValidationError";
import { refreshAccessToken } from "./authApi";

const API_URL = "/api/Contacts";

export async function getContacts() {
  const res = await fetch(`${BASE_URL}${API_URL}`, { headers: authHeader() });
  if (!res.ok) throw new Error("Failed to fetch contacts");
  return res.json();
}

export async function getContactById(id) {
  const res = await fetch(`${BASE_URL}${API_URL}/${id}`, { headers: authHeader() });
  if (!res.ok) throw new Error("Failed to fetch contact");
  return res.json();
}

export async function createContact(contact) {
  let res = await fetch(`${BASE_URL}${API_URL}`, {
    method: "POST",
    headers: { "Content-Type": "application/json", ...authHeader() },
    body: JSON.stringify(contact),
  });

  if (res.status === 401 || res.status === 403) {
    // probably expired token -> refresh it and try again
    await refreshAccessToken();
    res = await fetch(`${BASE_URL}${API_URL}`, {
      method: "POST",
      headers: { "Content-Type": "application/json", ...authHeader() },
      body: JSON.stringify(contact),
    });
  }

  if (res.status === 400) {
    throw new ValidationError(await res.json());
  }

  if (!res.ok) {
    throw new Error("Unexpected error");
  }

  return res.json();
}

export async function updateContact(id, contact) {
  let res = await fetch(`${BASE_URL}${API_URL}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json", ...authHeader() },
    body: JSON.stringify(contact),
  });

  if (res.status === 401 || res.status === 403) {
    // probably expired token -> refresh it and try again
    await refreshAccessToken();
    res = await fetch(`${BASE_URL}${API_URL}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json", ...authHeader() },
      body: JSON.stringify(contact),
    });
  }

  if (res.status === 404) {
    throw new Error("Contact not found");
  }

  if (res.status === 400) {
    throw new ValidationError(await res.json());
  }

  if (!res.ok) {
    throw new Error("Unexpected error");
  }

  return res.json();
}

export async function deleteContact(id) {
  let res = await fetch(`${BASE_URL}${API_URL}/${id}`, { method: "DELETE", headers: authHeader() });

  if (res.status === 401 || res.status === 403) {
    // probably expired token -> refresh it and try again
    await refreshAccessToken();
    res = await fetch(`${BASE_URL}${API_URL}/${id}`, { method: "DELETE", headers: authHeader() });
  }

  if (!res.ok) {
    throw new Error("Unexpected error");
  }
}

function authHeader() {
  const token = localStorage.getItem("token");
  return token ? { Authorization: `Bearer ${token}` } : {};
}
