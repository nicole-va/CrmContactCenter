<!-- ============================================================
  src/views/InteractionsView.vue
============================================================ -->
<!-- ============================================================
  src/views/FollowUpsView.vue
============================================================ -->
<template>
  <div>
    <div class="page-header">
      <h1>Seguimientos</h1>
      <Button label="Nuevo Seguimiento" icon="pi pi-plus" @click="openCreate" />
    </div>

    <!-- Filtros -->
    <div class="card" style="margin-bottom:16px">
      <div class="filters">
        <Select
          v-model="filters.status"
          :options="statusOptions"
          optionLabel="label"
          optionValue="value"
          @change="loadFollowUps"
          class="filter-select"
        />
        <Button label="Próximos 7 días" icon="pi pi-calendar" severity="secondary" @click="loadUpcoming" />
        <Button label="Todos" icon="pi pi-list" severity="secondary" @click="loadAll" />
      </div>
    </div>

    <!-- Tabla -->
    <div class="card">
      <DataTable
        :value="followUps"
        :loading="loading"
        stripedRows
        paginator
        :rows="10"
        :rowsPerPageOptions="[5, 10, 20]"
        emptyMessage="No hay seguimientos registrados"
        responsiveLayout="scroll"
      >
        <Column field="customerName"  header="Cliente"   />
        <Column field="customerPhone" header="Teléfono"  />
        <Column field="agentName"     header="Agente"    />
        <Column field="scheduledDate" header="Fecha"     />
        <Column field="status"        header="Estado">
          <template #body="{ data }">
            <span class="badge" :class="statusBadge(data.status)">{{ data.status }}</span>
          </template>
        </Column>
        <Column field="notes" header="Notas">
          <template #body="{ data }">{{ data.notes || '—' }}</template>
        </Column>
        <Column header="Acciones">
          <template #body="{ data }">
            <Button
              v-if="data.status === 'pendiente'"
              label="Completar"
              icon="pi pi-check"
              size="small"
              severity="success"
              @click="complete(data.id)"
            />
          </template>
        </Column>
      </DataTable>
    </div>

    <!-- Modal -->
    <Dialog
      v-model:visible="showModal"
      header="Nuevo Seguimiento"
      modal
      :style="{ width: '95%', maxWidth: '480px' }"
      :draggable="false"
    >
      <div class="form-group">
        <label>Cliente *</label>
        <Select
          v-model="form.customerId"
          :options="customers"
          optionLabel="fullName"
          optionValue="id"
          placeholder="Seleccionar cliente"
          filter
          class="w-full"
        />
      </div>
      <div class="form-group">
        <label>Fecha de seguimiento *</label>
        <InputText v-model="form.scheduledDate" type="date" class="w-full" />
      </div>
      <div class="form-group">
        <label>Notas</label>
        <Textarea v-model="form.notes" rows="3" placeholder="Motivo del seguimiento..." class="w-full" />
      </div>

      <template #footer>
        <Button label="Cancelar" severity="secondary" @click="showModal = false" />
        <Button label="Crear seguimiento" icon="pi pi-check" @click="saveFollowUp" />
      </template>
    </Dialog>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import api from '../services/api'
import Button    from 'primevue/button'
import InputText from 'primevue/inputtext'
import DataTable from 'primevue/datatable'
import Column    from 'primevue/column'
import Dialog    from 'primevue/dialog'
import Select    from 'primevue/select'
import Textarea  from 'primevue/textarea'

const followUps = ref([])
const customers = ref([])
const showModal = ref(false)
const loading   = ref(false)
const filters   = ref({ status: 'pendiente' })

const form = ref({ customerId: '', agentId: 1, scheduledDate: '', notes: '' })

const statusOptions = [
  { label: 'Pendiente',  value: 'pendiente'  },
  { label: 'Completado', value: 'completado' },
  { label: 'Cancelado',  value: 'cancelado'  }
]

async function loadFollowUps() {
  loading.value = true
  const params = {}
  if (filters.value.status) params.status = filters.value.status
  const res = await api.get('/followups', { params })
  followUps.value = res.data
  loading.value = false
}

async function loadUpcoming() {
  loading.value = true
  const res = await api.get('/followups/upcoming')
  followUps.value = res.data
  loading.value = false
}

async function loadAll() {
  filters.value.status = ''
  loadFollowUps()
}

async function openCreate() {
  const res = await api.get('/customers')
  customers.value = res.data.map(c => ({ ...c, fullName: `${c.firstName} ${c.lastName}` }))
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

<style scoped>
.w-full { width: 100%; }

.filters {
  display: flex;
  gap: 12px;
  flex-wrap: wrap;
  align-items: center;
}

.filter-select { min-width: 160px; }

@media (max-width: 600px) {
  .filter-select { width: 100%; }
}
</style>
