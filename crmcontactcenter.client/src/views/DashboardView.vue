<!-- src/views/DashboardView.vue -->
<template>
  <div>
    <div class="page-header">
      <h1>Dashboard</h1>
      <span class="date">{{ today }}</span>
    </div>

    <!-- Métricas principales -->
    <div class="metrics-grid">
      <div class="metric-card blue">
        <div class="metric-icon"><i class="pi pi-users"></i></div>
        <div class="metric-info">
          <p class="metric-label">Total Clientes</p>
          <p class="metric-value">{{ metrics.totalCustomers }}</p>
        </div>
      </div>

      <div class="metric-card yellow">
        <div class="metric-icon"><i class="pi pi-dollar"></i></div>
        <div class="metric-info">
          <p class="metric-label">Deuda Pendiente</p>
          <p class="metric-value">${{ formatAmount(metrics.totalPending) }}</p>
        </div>
      </div>

      <div class="metric-card red">
        <div class="metric-icon"><i class="pi pi-exclamation-triangle"></i></div>
        <div class="metric-info">
          <p class="metric-label">Deuda Vencida</p>
          <p class="metric-value">${{ formatAmount(metrics.totalOverdue) }}</p>
        </div>
      </div>

      <div class="metric-card green">
        <div class="metric-icon"><i class="pi pi-comments"></i></div>
        <div class="metric-info">
          <p class="metric-label">Interacciones Hoy</p>
          <p class="metric-value">{{ metrics.interactionsToday }}</p>
        </div>
      </div>
    </div>

    <!-- Resultados del día -->
    <div class="card" style="margin-top:24px">
      <h2 class="section-title">Resultados de hoy</h2>
      <div class="results-grid">
        <div class="result-item green">
          <i class="pi pi-check-circle result-icon"></i>
          <p class="result-value">{{ metrics.contacted }}</p>
          <p class="result-label">Contactados</p>
        </div>
        <div class="result-item red">
          <i class="pi pi-times-circle result-icon"></i>
          <p class="result-value">{{ metrics.noAnswer }}</p>
          <p class="result-label">No responde</p>
        </div>
        <div class="result-item yellow">
          <i class="pi pi-spinner-dotted result-icon"></i>
          <p class="result-value">{{ metrics.promiseToPay }}</p>
          <p class="result-label">Promesa de pago</p>
        </div>
        <div class="result-item blue">
          <i class="pi pi-wallet result-icon"></i>
          <p class="result-value">{{ metrics.paymentDone }}</p>
          <p class="result-label">Pagos realizados</p>
        </div>
        <div class="result-item orange">
          <i class="pi pi-bell result-icon"></i>
          <p class="result-value">{{ metrics.pendingFollowUps }}</p>
          <p class="result-label">Follow-ups pendientes</p>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue'
  import api from '../services/api'

  const metrics = ref({
    totalCustomers: 0, totalPending: 0, totalOverdue: 0,
    interactionsToday: 0, contacted: 0, noAnswer: 0,
    promiseToPay: 0, paymentDone: 0, pendingFollowUps: 0
  })

  const today = new Date().toLocaleDateString('es-EC', {
    weekday: 'long', year: 'numeric', month: 'long', day: 'numeric'
  })

  function formatAmount(val) {
    return Number(val || 0).toLocaleString('es-EC', { minimumFractionDigits: 2 })
  }

  onMounted(async () => {
    const res = await api.get('/dashboard')
    metrics.value = res.data
  })
</script>

<style scoped>
  .date {
    font-size: 0.875rem;
    color: #6b7280;
    text-transform: capitalize;
  }

  .section-title {
    font-size: 1rem;
    font-weight: 600;
    margin-bottom: 16px;
    color: #1e2a3a;
  }

  /* ── Métricas ── */
  .metrics-grid {
    display: grid;
    grid-template-columns: repeat(4, 1fr);
    gap: 16px;
  }

  .metric-card {
    background: white;
    border-radius: 10px;
    padding: 20px;
    display: flex;
    align-items: center;
    gap: 16px;
    box-shadow: 0 1px 4px rgba(0,0,0,0.08);
    border-left: 4px solid transparent;
  }

    .metric-card.blue {
      border-color: #3b82f6;
    }

    .metric-card.yellow {
      border-color: #f59e0b;
    }

    .metric-card.red {
      border-color: #ef4444;
    }

    .metric-card.green {
      border-color: #10b981;
    }

  .metric-icon {
    width: 48px;
    height: 48px;
    border-radius: 10px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 1.3rem;
  }

  .blue .metric-icon {
    background: #dbeafe;
    color: #3b82f6;
  }

  .yellow .metric-icon {
    background: #fef3c7;
    color: #f59e0b;
  }

  .red .metric-icon {
    background: #fee2e2;
    color: #ef4444;
  }

  .green .metric-icon {
    background: #d1fae5;
    color: #10b981;
  }

  .metric-label {
    font-size: 0.75rem;
    color: #6b7280;
    margin-bottom: 4px;
  }

  .metric-value {
    font-size: 1.5rem;
    font-weight: 700;
    color: #1e2a3a;
  }

  /* ── Resultados ── */
  .results-grid {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 12px;
  }

  .result-item {
    border-radius: 10px;
    padding: 20px 12px;
    text-align: center;
  }

    .result-item.green {
      background: #d1fae5;
    }

    .result-item.red {
      background: #fee2e2;
    }

    .result-item.yellow {
      background: #fef3c7;
    }

    .result-item.blue {
      background: #dbeafe;
    }

    .result-item.orange {
      background: #ffedd5;
    }

  .result-icon {
    font-size: 1.5rem;
    margin-bottom: 8px;
    display: block;
  }

  .green .result-icon {
    color: #10b981;
  }

  .red .result-icon {
    color: #ef4444;
  }

  .yellow .result-icon {
    color: #f59e0b;
  }

  .blue .result-icon {
    color: #3b82f6;
  }

  .orange .result-icon {
    color: #f97316;
  }

  .result-value {
    font-size: 2rem;
    font-weight: 700;
    color: #1e2a3a;
  }

  .result-label {
    font-size: 0.75rem;
    color: #475569;
    margin-top: 4px;
  }

  /* ── Responsivo ── */
  @media (max-width: 1024px) {
    .metrics-grid {
      grid-template-columns: repeat(2, 1fr);
    }

    .results-grid {
      grid-template-columns: repeat(3, 1fr);
    }
  }

  @media (max-width: 600px) {
    .metrics-grid {
      grid-template-columns: 1fr;
    }

    .results-grid {
      grid-template-columns: repeat(2, 1fr);
    }

    .metric-value {
      font-size: 1.2rem;
    }

    .page-header {
      flex-direction: column;
      align-items: flex-start;
    }
  }
</style>
