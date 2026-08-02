const API_BASE_URL = '/api';

const postJson = async (url, payload) => {
  const response = await fetch(`${API_BASE_URL}${url}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload)
  });

  if (!response.ok) {
    throw new Error('Request failed');
  }

  return response.json();
};

const getJson = async (url) => {
  const response = await fetch(`${API_BASE_URL}${url}`);
  if (!response.ok) {
    throw new Error('Request failed');
  }
  return response.json();
};
