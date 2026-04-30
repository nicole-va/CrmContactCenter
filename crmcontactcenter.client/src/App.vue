<template>
  <div id="app">
    <!-- Layout con sidebar si está autenticado -->
    <div v-if="auth.isAuthenticated" class="layout">

      <!-- Sidebar -->
      <aside class="sidebar">
        <div class="sidebar-header">
          <h2>📞 CRM</h2>
          <p>Contact Center</p>
        </div>

        <nav class="sidebar-nav">
          <RouterLink to="/dashboard">
            <span>📊</span> Dashboard
          </RouterLink>
          <RouterLink to="/customers">
            <span>👥</span> Clientes
          </RouterLink>
          <RouterLink to="/interactions">
            <span>📋</span> Interacciones
          </RouterLink>
          <RouterLink to="/followups">
            <span>🔔</span> Seguimientos
          </RouterLink>
        </nav>

        <div class="sidebar-footer">
          <p class="user-name">{{ auth.fullName }}</p>
          <p class="user-role">{{ auth.user?.role }}</p>
          <button @click="logout" class="btn-logout">Cerrar sesión</button>
        </div>
      </aside>

      <!-- Contenido principal -->
      <main class="main-content">
        <RouterView />
      </main>
    </div>

    <!-- Solo el contenido si no está autenticado -->
    <div v-else>
      <RouterView />
    </div>
  </div>
</template>

<script setup>
  import { useAuthStore } from './stores/auth'
  import { useRouter } from 'vue-router'

  const auth = useAuthStore()
  const router = useRouter()

  function logout() {
    auth.logout()
    router.push('/login')
  }
</script>

<style>
  * {
    margin: 0;
    padding: 0;
    box-sizing: border-box;
  }

  body {
    font-family: 'Segoe UI', sans-serif;
    background: #f0f2f5;
    color: #333;
  }

  .layout {
    display: flex;
    min-height: 100vh;
  }

  /* ── Sidebar ── */
  .sidebar {
    width: 240px;
    background: #1e2a3a;
    color: white;
    display: flex;
    flex-direction: column;
    position: fixed;
    top: 0;
    left: 0;
    height: 100vh;
  }

  .sidebar-header {
    padding: 24px 20px;
    border-bottom: 1px solid #2d3f54;
  }

    .sidebar-header h2 {
      font-size: 1.3rem;
      font-weight: 700;
    }

    .sidebar-header p {
      font-size: 0.75rem;
      color: #8a9bb0;
      margin-top: 2px;
    }

  .sidebar-nav {
    flex: 1;
    padding: 16px 0;
  }

    .sidebar-nav a {
      display: flex;
      align-items: center;
      gap: 10px;
      padding: 12px 20px;
      color: #8a9bb0;
      text-decoration: none;
      font-size: 0.9rem;
      transition: all 0.2s;
    }

      .sidebar-nav a:hover,
      .sidebar-nav a.router-link-active {
        background: #2d3f54;
        color: white;
      }

  .sidebar-footer {
    padding: 16px 20px;
    border-top: 1px solid #2d3f54;
  }

  .user-name {
    font-size: 0.85rem;
    font-weight: 600;
  }

  .user-role {
    font-size: 0.75rem;
    color: #8a9bb0;
    margin-bottom: 10px;
  }

  .btn-logout {
    width: 100%;
    padding: 8px;
    background: #e53e3e;
    color: white;
    border: none;
    border-radius: 6px;
    cursor: pointer;
    font-size: 0.85rem;
  }

    .btn-logout:hover {
      background: #c53030;
    }

  /* ── Main Content ── */
  .main-content {
    margin-left: 240px;
    flex: 1;
    padding: 24px;
    min-height: 100vh;
  }

  /* ── Utilidades globales ── */
  .card {
    background: white;
    border-radius: 10px;
    padding: 20px;
    box-shadow: 0 1px 4px rgba(0,0,0,0.08);
  }

  .page-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

    .page-header h1 {
      font-size: 1.4rem;
      font-weight: 700;
    }

  .btn {
    padding: 8px 16px;
    border-radius: 6px;
    border: none;
    cursor: pointer;
    font-size: 0.875rem;
    font-weight: 500;
    transition: opacity 0.2s;
  }

    .btn:hover {
      opacity: 0.85;
    }

  .btn-primary {
    background: #3b82f6;
    color: white;
  }

  .btn-success {
    background: #10b981;
    color: white;
  }

  .btn-danger {
    background: #ef4444;
    color: white;
  }

  .btn-warning {
    background: #f59e0b;
    color: white;
  }

  .btn-secondary {
    background: #6b7280;
    color: white;
  }

  table {
    width: 100%;
    border-collapse: collapse;
    font-size: 0.875rem;
  }

  th {
    text-align: left;
    padding: 10px 12px;
    background: #f8fafc;
    border-bottom: 2px solid #e2e8f0;
    font-weight: 600;
    color: #475569;
  }

  td {
    padding: 10px 12px;
    border-bottom: 1px solid #f1f5f9;
  }

  tr:hover td {
    background: #f8fafc;
  }

  .badge {
    display: inline-block;
    padding: 2px 10px;
    border-radius: 99px;
    font-size: 0.75rem;
    font-weight: 600;
  }

  .badge-green {
    background: #d1fae5;
    color: #065f46;
  }

  .badge-red {
    background: #fee2e2;
    color: #991b1b;
  }

  .badge-yellow {
    background: #fef3c7;
    color: #92400e;
  }

  .badge-blue {
    background: #dbeafe;
    color: #1e40af;
  }

  .badge-gray {
    background: #f1f5f9;
    color: #475569;
  }

  input, select, textarea {
    width: 100%;
    padding: 8px 12px;
    border: 1px solid #e2e8f0;
    border-radius: 6px;
    font-size: 0.875rem;
    outline: none;
    transition: border-color 0.2s;
  }

    input:focus, select:focus, textarea:focus {
      border-color: #3b82f6;
    }

  .form-group {
    margin-bottom: 14px;
  }

    .form-group label {
      display: block;
      font-size: 0.8rem;
      font-weight: 600;
      color: #475569;
      margin-bottom: 4px;
    }

  /* ── Modal ── */
  .modal-overlay {
    position: fixed;
    inset: 0;
    background: rgba(0,0,0,0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
  }

  .modal {
    background: white;
    border-radius: 12px;
    padding: 24px;
    width: 480px;
    max-width: 95vw;
    max-height: 90vh;
    overflow-y: auto;
  }

  .modal-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
  }

    .modal-header h2 {
      font-size: 1.1rem;
      font-weight: 700;
    }

  .modal-close {
    background: none;
    border: none;
    font-size: 1.2rem;
    cursor: pointer;
    color: #6b7280;
  }

  .modal-footer {
    display: flex;
    gap: 8px;
    justify-content: flex-end;
    margin-top: 20px;
  }
</style>
