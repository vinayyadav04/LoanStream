const collectLeadPayload = (form) => {
  const formData = new FormData(form);
  return {
    name: String(formData.get('name') || '').trim(),
    email: String(formData.get('email') || '').trim(),
    phone: String(formData.get('phone') || '').trim(),
    employmentType: String(formData.get('employmentType') || '').trim(),
    monthlyIncome: String(formData.get('monthlyIncome') || '').trim(),
    loanAmount: Number(formData.get('loanAmount') || 0),
    city: String(formData.get('city') || '').trim(),
    source: window.location.href
  };
};

const collectContactPayload = (form) => {
  const formData = new FormData(form);
  return {
    name: String(formData.get('name') || '').trim(),
    email: String(formData.get('email') || '').trim(),
    phone: String(formData.get('phone') || '').trim(),
    topic: String(formData.get('topic') || '').trim(),
    message: String(formData.get('message') || '').trim()
  };
};

document.getElementById('leadForm')?.addEventListener('submit', async (event) => {
  event.preventDefault();
  const form = event.currentTarget;
  const payload = collectLeadPayload(form);
  try {
    const response = await postJson('/api/leads', payload);
    if (response?.success) {
      form.reset();
      window.location.assign('/thank-you.html?source=application');
    } else {
      showModal('Submission issue', response?.message || 'Unexpected response from the server.');
    }
  } catch (error) {
    console.error('Lead submission failed', error);
    showModal('Submission issue', 'We could not reach the backend. Please try again shortly.');
  }
});

document.getElementById('contactForm')?.addEventListener('submit', async (event) => {
  event.preventDefault();
  const form = event.currentTarget;
  const payload = collectContactPayload(form);
  try {
    await postJson('/api/contact', payload);
    form.reset();
    window.location.assign('/thank-you.html?source=contact');
  } catch (error) {
    showModal('Submission issue', 'We could not reach the backend. Please try again shortly.');
  }
});

document.getElementById('applyNavBtn')?.addEventListener('click', () => {
  document.querySelector('#leadForm input[name="name"]').focus();
});
document.getElementById('applyHeroBtn')?.addEventListener('click', () => {
  document.querySelector('#leadForm input[name="name"]').focus();
});
