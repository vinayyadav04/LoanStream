const collectLeadPayload = (form) => {
  const formData = new FormData(form);

  return {
    name: String(formData.get("name") || "").trim(),
    email: String(formData.get("email") || "").trim(),
    phone: String(formData.get("phone") || "").trim(),
    employmentType: String(formData.get("employmentType") || "").trim(),
    monthlyIncome: String(formData.get("monthlyIncome") || "").trim(),
    loanAmount: Number(formData.get("loanAmount") || 0),
    city: String(formData.get("city") || "").trim(),
    source: window.location.href
  };
};

const collectContactPayload = (form) => {
  const formData = new FormData(form);

  return {
    name: String(formData.get("name") || "").trim(),
    email: String(formData.get("email") || "").trim(),
    phone: String(formData.get("phone") || "").trim(),
    topic: String(formData.get("topic") || "").trim(),
    message: String(formData.get("message") || "").trim()
  };
};

// Regular Expressions
const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
const phoneRegex = /^[6-9]\d{9}$/;

// =========================
// Lead Form
// =========================

const leadForm = document.getElementById("leadForm");

if (leadForm) {

  const emailInput = leadForm.querySelector('[name="email"]');
  const phoneInput = leadForm.querySelector('[name="phone"]');

  // Remove errors while typing
  emailInput.addEventListener("input", () => {
    emailInput.classList.remove("error");
    document.getElementById("emailError").textContent = "";
  });

  phoneInput.addEventListener("input", () => {
    phoneInput.classList.remove("error");
    document.getElementById("phoneError").textContent = "";
  });

  leadForm.addEventListener("submit", async (event) => {

    event.preventDefault();

    const payload = collectLeadPayload(leadForm);

    // Clear old errors
    document.getElementById("emailError").textContent = "";
    document.getElementById("phoneError").textContent = "";

    emailInput.classList.remove("error");
    phoneInput.classList.remove("error");

    let isValid = true;

    if (!emailRegex.test(payload.email)) {
      document.getElementById("emailError").textContent =
        "Please enter a valid email address.";
      emailInput.classList.add("error");
      isValid = false;
    }

    if (!phoneRegex.test(payload.phone)) {
      document.getElementById("phoneError").textContent =
        "Please enter a valid 10-digit mobile number.";
      phoneInput.classList.add("error");
      isValid = false;
    }

    if (!isValid) return;

    try {

      const response = await postJson("/api/leads", payload);

      if (response?.success) {
        leadForm.reset();
        window.location.assign("/thank-you.html?source=application");
      } else {
        showModal(
          "Submission issue",
          response?.message || "Unexpected response from the server."
        );
      }

    } catch (error) {

      console.error("Lead submission failed", error);

      showModal(
        "Submission issue",
        "We could not reach the backend. Please try again shortly."
      );
    }

  });

}

// =========================
// Contact Form
// =========================

const contactForm = document.getElementById("contactForm");

if (contactForm) {

  contactForm.addEventListener("submit", async (event) => {

    event.preventDefault();

    const payload = collectContactPayload(contactForm);

    if (!emailRegex.test(payload.email)) {
      showModal(
        "Validation Error",
        "Please enter a valid email address."
      );
      return;
    }

    if (!phoneRegex.test(payload.phone)) {
      showModal(
        "Validation Error",
        "Please enter a valid 10-digit mobile number."
      );
      return;
    }

    try {

      await postJson("/api/contact", payload);

      contactForm.reset();

      window.location.assign("/thank-you.html?source=contact");

    } catch (error) {

      showModal(
        "Submission issue",
        "We could not reach the backend. Please try again shortly."
      );

    }

  });

}

// =========================
// Buttons
// =========================

document.getElementById("applyNavBtn")?.addEventListener("click", () => {
  document.querySelector('#leadForm input[name="name"]').focus();
});

document.getElementById("applyHeroBtn")?.addEventListener("click", () => {
  document.querySelector('#leadForm input[name="name"]').focus();
});