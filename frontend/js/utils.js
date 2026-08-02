const showModal = (title, message) => {
  const modal = document.getElementById('modal');
  const titleEl = document.getElementById('modalTitle');
  const messageEl = document.getElementById('modalMessage');
  titleEl.textContent = title;
  messageEl.textContent = message;
  if (modal) {
    modal.style.display = 'flex';
    modal.classList.add('open');
  }
};

const closeModal = () => {
  const modal = document.getElementById('modal');
  if (modal) {
    modal.classList.remove('open');
    modal.style.display = 'none';
  }
};

document.getElementById('closeModalBtn')?.addEventListener('click', closeModal);
document.getElementById('modal')?.addEventListener('click', (event) => {
  if (event.target.id === 'modal') {
    closeModal();
  }
});
