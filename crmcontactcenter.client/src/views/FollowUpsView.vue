<template>
  <div>
    <div class="page-header">
      <h1>🔔 Seguimientos</h1>
      <button class="btn btn-primary" @click="openCreate">+ Nuevo Seguimiento</button>
    </div>

    <!-- Filtros -->
    <div class="card" style="margin-bottom:16px; display:flex; gap:12px; flex-wrap:wrap; align-items:center;">
      <select v-model="filters.status" @change="loadFollowUps" style="width:auto">
        <option value="">Todos</option>
        <option value="pendiente">Pendiente</option>
        <option value="completado">Completado</option>
        <option value="cancelado">Cancelado</option>
      </select>
      <button class="btn btn-secondary" @click="loadUpcoming">📅 Próximos 7 días</button>
    </div>

    <!-- Tabla -->
    <div class="card">
      <table>
        <thead>
          <tr>
            <th>Cliente</th>
            <th>Teléfono</th>
            <th>Agente</th>
            <th>Fecha</th>
            <th>Estado</th>
            <th>Notas</th>
            <th>Acciones</th>
          </tr>
        </thead>
        <tbody>
          <tr v-if="followUps.length === 0">
            <td colspan="7" style="text-align:center; color:#6b7280;">No hay seguimientos registrados</td>
          </tr>
          <tr v-for="f in followUps" :key="f.id">
            <td>{{ f.customerName }}</td>
            <td>{{ f.customerPhone || '—' }}</td>
            <td>{{ f.agentName }}</td>
            <td>{{ f.scheduledDate }}</td>
            <td>
              <span class="badge" :class="statusBadge(f.status)">{{ f.status }}</span>
            </td>
            <td>{{ f.notes || '—' }}</td>
            <td>
              <button v-if="f.status === 'pendiente'"
                      class="btn btn-success"
                      style="font-size:0.75rem; padding:4px 10px"
                      @click="complete(f.id)">
                ✓ Completar
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Modal Nuevo Follow-up -->
    <div v-if="showModal" class="modal-overlay" @click.self="showModal = false">
      <div class="modal">
        <div class="modal-header">
          <h2>Nuevo Seguimiento</h2>
          <button class="modal-close" @click="showModal = false">✕</button>
        </div>

        <div class="form-group">
          <label>Cliente *</label>
          <select v-model="form.customerId">
            <option value="">Seleccionar cliente</option>
            <option v-for="c in customers" :key="c.id" :value="c.id">
              {{ c.firstName }} {{ c.lastName }}
            </option>
          </select>
        </div>
        <div class="form-group">
          <label>Fecha de seguimiento *</label>
          <input v-model="form.scheduledDate" type="date" />
        </div>
        <div class="form-group">
          <label>Notas</label>
          <textarea v-model="form.notes" rows="3" placeholder="Motivo del seguimiento..."></textarea>
        </div>

        <div class="modal-footer">
          <button class="btn btn-secondary" @click="showModal = false">Cancelar</button>
          <button class="btn btn-primary" @click="saveFollowUp">Crear seguimiento</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api'

const followUps = ref([])
const customers = ref([])
const showModal = ref(false)
const filters   = ref({ status: 'pendiente' })

const form = ref({ customerId: '', agentId: 1, scheduledDate: '', notes: '' })

async function loadFollowUps() {
  const params = {}
  if (filters.value.status) params.status = filters.value.status
  const res = await api.get('/followups', { params })
  followUps.value = res.data
}

async function loadUpcoming() {
  const res = await api.get('/followups/upcoming')
  followUps.value = res.data
}

async function openCreate() {
  const res = await api.get('/customers')
  customers.value = res.data
  form.value = { customerId: '', agentId: 1, scheduledDate: '', notes: '' }
  showModal.value = true
}

async function saveFollowUp() {
  await api.post('/followups', form.value)
  showModal.value = false
  loadFollowUps()
}

async function complete(id) {
  await api.patch(`/followups/${id}/status`, null, { params: { status: 'completado' } })
  loadFollowUps()
}

function statusBadge(status) {
  return { pendiente: 'badge-yellow', completado: 'badge-green', cancelado: 'badge-red' }[status] || 'badge-gray'
}

onMounted(loadFollowUps)
</script>
