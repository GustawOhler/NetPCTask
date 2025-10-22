export default class ValidationError extends Error {
  /**
   * Creates a new ValidationError from a backend 400 response.
   * @param {Object} response - Parsed JSON body from the API.
   */
  constructor(response) {
    const message =
      response?.title ||
      "Validation failed. Please correct the fields.";

    super(message);
    this.name = "ValidationError";
    this.status = 400;
    this.errors = response?.errors || {};
  }

  /**
   * Returns true if any validation errors exist.
   */
  hasErrors() {
    return Object.keys(this.errors).length > 0;
  }

  /**
   * Returns flat array of readable messages like "Email: Invalid format".
   */
  getMessages() {
    if (!this.hasErrors()) return [];
    return Object.entries(this.errors)
      .flatMap(([field, messages]) => messages.map(msg => `${field}: ${msg}`));
  }
}
