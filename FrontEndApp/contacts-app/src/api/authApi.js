import { BASE_URL } from "../helpers/constants";
import ValidationError from "../helpers/ValidationError";

const API_PATH = "/api/Auth";
const FULL_PATH = `${BASE_URL}${API_PATH}`;

export async function loginOnServer(username, password) {
  const res = await fetch(`${FULL_PATH}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify({ userName: username, password }),
  });
  if (res.status === 404) {
    throw new Error("User does not exist");
  }
  if (!res.ok) {
    throw new Error("Login failed");
  }
  const data = await res.json();
  return data;
}

export async function refreshAccessToken() {
  const res = await fetch(`${FULL_PATH}/refresh`, {
    method: "POST",
    credentials: "include", // cookie zostaje wysłane
  });
  if (!res.ok) throw new Error("Token refresh failed");
  const data = await res.json();
  localStorage.setItem("token", data.accessToken);
  return data;
}

export async function registerOnServer(user) {
  const res = await fetch(`${FULL_PATH}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(user),
  });
  if (res.status === 400) {
    throw new ValidationError(await res.json());
  }
  if (!res.ok) throw new Error("Registration failed");
}
