const API_BASE_URL = '';
const fakeCredentials = { username: 'admin', password: 'admin123' };

document.addEventListener('DOMContentLoaded', () => {
  console.log('Admin dashboard initialized');
  const loginView = document.getElementById('loginView');
  const dashboardView = document.getElementById('dashboardView');
  const loginForm = document.getElementById('loginForm');
  const logoutBtn = document.getElementById('logoutBtn');
  const filterBtn = document.getElementById('filterBtn');
  const leadRows = document.getElementById('leadRows');
  const excelLink = document.getElementById('excelLink');
  const csvLink = document.getElementById('csvLink');
  const loginError = document.getElementById('loginError');
  const statusBanner = document.getElementById('adminStatus');

  if (!loginForm) {
    console.error('Admin login form not found');
    return;
  }

  if (excelLink) {
    excelLink.href = `${API_BASE_URL}/api/admin/export/excel`;
  }
  if (csvLink) {
    csvLink.href = `${API_BASE_URL}/api/admin/export/csv`;
  }

  const setAuthenticated = (value) => {
    if (loginView) {
      loginView.hidden = value;
      loginView.style.display = value ? 'none' : 'flex';
    }
    if (dashboardView) {
      dashboardView.hidden = !value;
      dashboardView.style.display = value ? 'block' : 'none';
    }
    if (statusBanner) {
      statusBanner.textContent = value ? 'Dashboard loaded successfully.' : 'Please login to continue.';
      statusBanner.style.display = 'block';
      statusBanner.style.color = value ? '#0f5132' : '#0f172a';
      statusBanner.style.background = value ? '#d1e7dd' : '#f8f9fa';
    }
  };

  setAuthenticated(false);

  loginForm.addEventListener('submit', (event) => {
    event.preventDefault();
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');

    const username = usernameInput?.value?.trim() || '';
    const password = passwordInput?.value?.trim() || '';

    if (!username || !password) {
      if (loginError) {
        loginError.textContent = 'Please enter both username and password.';
        loginError.hidden = false;
      }
      return;
    }

    if (username === fakeCredentials.username && password === fakeCredentials.password) {
      if (loginError) loginError.hidden = true;
      setAuthenticated(true);
      loadLeads(leadRows);
    } else {
      if (loginError) {
        loginError.textContent = 'Invalid username or password.';
        loginError.hidden = false;
      }
    }
  });

  logoutBtn?.addEventListener('click', () => {
    setAuthenticated(false);
    loginForm.reset();
    if (loginError) loginError.hidden = true;
  });

  filterBtn?.addEventListener('click', () => loadLeads(leadRows));
});

async function loadLeads(leadRows) {
  if (!leadRows) {
    console.error('Lead rows element not found');
    return;
  }

  const name = document.getElementById('searchName')?.value || '';
  const phone = document.getElementById('searchPhone')?.value || '';
  const fromDate = document.getElementById('fromDate')?.value || '';
  const toDate = document.getElementById('toDate')?.value || '';
  const params = new URLSearchParams();
  if (name) params.set('name', name);
  if (phone) params.set('phone', phone);
  if (fromDate) params.set('fromDate', fromDate);
  if (toDate) params.set('toDate', toDate);

  try {
    const response = await fetch(`${API_BASE_URL}/api/admin/leads?${params.toString()}`);
    if (!response.ok) {
      console.error('Failed to load leads', response.statusText);
      leadRows.innerHTML = '<tr><td colspan="8">Unable to load leads.</td></tr>';
      return;
    }
    const leads = await response.json();
    leadRows.innerHTML = leads.map((lead) => `
      <tr>
        <td>${lead.name}</td>
        <td>${lead.phone}</td>
        <td>${lead.email}</td>
        <td>${lead.city}</td>
        <td>${lead.monthlyIncome}</td>
        <td>${lead.loanAmount}</td>
        <td>${lead.status}</td>
        <td>${new Date(lead.createdDate).toLocaleString()}</td>
      </tr>
    `).join('');
  } catch (error) {
    console.error('Lead fetch error', error);
    leadRows.innerHTML = '<tr><td colspan="8">Unable to load leads.</td></tr>';
  }
}
