<!-- src/views/CustomersView.vue -->
<template>
  <div>
    <div class="page-header">
      <h1>Clientes</h1>
      <Button label="Nuevo Cliente" icon="pi pi-plus" @click="openCreate" />
    </div>

    <!-- Búsqueda -->
    <div class="card" style="margin-bottom:16px">
      <IconField>
        <InputIcon class="pi pi-search" />
        <InputText v-model="search"
                   placeholder="Buscar por nombre, cédula o teléfono..."
                   @input="loadCustomers"
                   class="w-full" />
      </IconField>
    </div>

    <!-- Tabla -->
    <div class="card">
      <div class="table-wrapper">
        <DataTable :value="customers"
                   :loading="loading"
                   stripedRows
                   paginator
                   :rows="10"
                   :rowsPerPageOptions="[5, 10, 20]"
                   emptyMessage="No hay clientes registrados"
                   responsiveLayout="scroll">
          <Column field="firstName" header="Nombre">
            <template #body="{ data }">
              {{ data.firstName }} {{ data.lastName }}
            </template>
          </Column>
          <Column field="cedula" header="Cédula" />
          <Column field="phone" header="Teléfono" />
          <Column field="city" header="Ciudad" />
          <Column field="status" header="Estado">
            <template #body="{ data }">
              <span class="badge" :class="data.status === 'activo' ? 'badge-green' : 'badge-red'">
                {{ data.status }}
              </span>
            </template>
          </Column>
          <Column header="Acciones">
            <template #body="{ data }">
              <div class="action-btns">
                <Button icon="pi pi-pencil" size="small" severity="warn" @click="openEdit(data)" />
                <Button icon="pi pi-trash" size="small" severity="danger" @click="deleteCustomer(data.id)" />
              </div>
            </template>
          </Column>
        </DataTable>
      </div>
    </div>

    <!-- Modal Crear/Editar -->
    <Dialog v-model:visible="showModal"
            :header="editing ? 'Editar Cliente' : 'Nuevo Cliente'"
            modal
            :style="{ width: '95%', maxWidth: '480px' }"
            :draggable="false">
      <div class="form-group">
        <label>Nombre *</label>
        <InputText v-model="form.firstName" placeholder="Nombre" class="w-full" />
      </div>
      <div class="form-group">
        <label>Apellido *</label>
        <InputText v-model="form.lastName" placeholder="Apellido" class="w-full" />
      </div>
      <div class="form-group">
        <label>Cédula</label>
        <InputText v-model="form.cedula" placeholder="1234567890" class="w-full" />
      </div>
      <div class="form-group">
        <label>Teléfono</label>
        <InputText v-model="form.phone" placeholder="0991234567" class="w-full" />
      </div>
      <div class="form-group">
        <label>Email</label>
        <InputText v-model="form.email" type="email" placeholder="correo@email.com" class="w-full" />
      </div>
      <div class="form-group">
        <label>Ciudad</label>
        <InputText v-model="form.city" placeholder="Guayaquil" class="w-full" />
      </div>
      <div class="form-group">
        <label>Dirección</label>
        <InputText v-model="form.address" placeholder="Av. Principal 123" class="w-full" />
      </div>
      <div v-if="editing" class="form-group">
        <label>Estado</label>
        <Select v-model="form.status"
                :options="statusOptions"
                optionLabel="label"
                optionValue="value"
                class="w-full" />
      </div>
      <div class="form-group">
        <label>Notas</label>
        <Textarea v-model="form.notes" rows="3" placeholder="Observaciones..." class="w-full" />
      </div>

      <template #footer>
        <Button label="Cancelar" severity="secondary" @click="showModal = false" />
        <Button :label="editing ? 'Guardar cambios' : 'Crear cliente'" @click="saveCustomer" />
      </template>
    </Dialog>
  </div>
</template>

<script setup>
  import { ref, onMounted } from 'vue'
  import api from '../services/api'
  import Button from 'primevue/button'
  import InputText from 'primevue/inputtext'
  import IconField from 'primevue/iconfield'
  import InputIcon from 'primevue/inputicon'
  import DataTable from 'primevue/datatable'
  import Column from 'primevue/column'
  import Dialog from 'primevue/dialog'
  import Select from 'primevue/select'
  import Textarea from 'primevue/textarea'

  const customers = ref([])
  const search = ref('')
  const showModal = ref(false)
  const editing = ref(false)
  const editId = ref(null)
  const loading = ref(false)

  const statusOptions = [
    { label: 'Activo', value: 'activo' },
    { label: 'Inactivo', value: 'inactivo' }
  ]

  const form = ref({
    firstName: '', lastName: '', cedula: '', phone: '',
    email: '', city: '', address: '', notes: '', status: 'activo'
  })

  async function loadCustomers() {
    loading.value = true
    const res = await api.get('/customers', { params: { search: search.value } })
    customers.value = res.data
    loading.value = false
  }

  function openCreate() {
    editing.value = false
    editId.value = null
    form.value = {
      firstName: '', lastName: '', cedula: '', phone: '',
      email: '', city: '', address: '', notes: '', status: 'activo'
    }
    showModal.value = true
  }

  function openEdit(c) {
    editing.value = true
    editId.value = c.id
    form.value = {
      firstName: c.firstName, lastName: c.lastName,
      cedula: c.cedula, phone: c.phone, email: c.email,
      city: c.city, address: c.address, notes: c.notes, status: c.status
    }
    showModal.value = true
  }

  async function saveCustomer() {
    if (editing.value) {
      await api.put(`/customers/${editId.value}`, form.value)
    } else {
      await api.post('/customers', form.value)
    }
    showModal.value = false
    loadCustomers()
  }

  async function deleteCustomer(id) {
    if (!confirm('¿Eliminar este cliente?')) return
    await api.delete(`/customers/${id}`)
    loadCustomers()
  }

  onMounted(loadCustomers)
</script>

<style scoped>
  .w-full {
    width: 100%;
  }

  .action-btns {
    display: flex;
    gap: 6px;
  }
</style>
