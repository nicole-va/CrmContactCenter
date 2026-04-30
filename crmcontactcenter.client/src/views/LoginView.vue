<!-- src/views/LoginView.vue -->
<template>
  <div class="page">

    <div class="shape shape-left"></div>
    <div class="shape shape-right"></div>

    <div class="dots dots-left"></div>
    <div class="dots dots-right"></div>

    <div class="login-card">

      <i class="pi pi-users" style="font-size:2.5rem; color:#3b82f6"</i>
      <h1>CRM Contact Center</h1>
      <p>Ingresa tus credenciales para continuar</p>

      <div class="form-group">

        <label>Correo electrónico</label>
        <InputText v-model="form.email"
                   type="email"
                   placeholder="admin@crm.com"
                   class="w-full" />
      </div>

      <div class="form-group">
        <label>Contraseña</label>
        <Password v-model="form.password"
                  placeholder="••••••••"
                  :feedback="false"
                  toggleMask
                  class="w-full" />
      </div>

      <Message v-if="error" severity="error" :closable="false">{{ error }}</Message>

      <Button label="Ingresar"
              icon="pi pi-sign-in"
              class="btn"
              :loading="loading"
              @click="handleLogin" />

    </div>
  </div>
</template>

<script setup>
  import { ref } from 'vue'
  import { useRouter } from 'vue-router'
  import { useAuthStore } from '../stores/auth'
  import InputText from 'primevue/inputtext'
  import Password from 'primevue/password'
  import Button from 'primevue/button'
  import Message from 'primevue/message'
  import 'primeicons/primeicons.css';

  const router = useRouter()
  const auth = useAuthStore()

  const form = ref({ email: '', password: '' })
  const error = ref('')
  const loading = ref(false)

  async function handleLogin() {
    error.value = ''
    loading.value = true
    try {
      await auth.login(form.value.email, form.value.password)
      router.push('/dashboard')
    } catch {
      error.value = 'Correo o contraseña incorrectos'
    } finally {
      loading.value = false
    }
  }
</script>

<style scoped>
  body {
    margin: 0;
    font-family: Arial, sans-serif;
  }

  .page {
    height: 100vh;
    width: 100vw;
    display: flex;
    justify-content: center;
    align-items: center;
    position: relative;
    overflow: hidden;
    background: radial-gradient(circle at 20% 20%, #1e3a8a, transparent 40%), radial-gradient(circle at 80% 80%, #1e40af, transparent 40%), linear-gradient(135deg, #0f172a, #020617);
  }

  /* Formas */
  .shape {
    position: absolute;
    width: 400px;
    height: 400px;
    border-radius: 50%;
    background: rgba(59, 130, 246, 0.2);
    filter: blur(120px);
  }

  .shape-left {
    top: -100px;
    left: -100px;
  }

  .shape-right {
    bottom: -100px;
    right: -100px;
  }

  /* Puntitos */
  .dots {
    position: absolute;
    width: 150px;
    height: 150px;
    background-image: radial-gradient(#3b82f6 2px, transparent 2px);
    background-size: 20px 20px;
    opacity: 0.3;
  }

  .dots-left {
    top: 60px;
    left: 60px;
  }

  .dots-right {
    bottom: 60px;
    right: 60px;
  }

  /* Card */
  .login-card {
    position: relative;
    z-index: 1;
    width: 25rem;
    padding: 2rem;
    border-radius: 20px;
    background: rgba(255, 255, 255, 0.95);
    backdrop-filter: blur(10px);
    box-shadow: 0 25px 60px rgba(0,0,0,0.3);
    text-align: center;
  }

    .login-card h1 {
      margin: 10px 0;
      font-size: 1.5rem;
      color: #1e293b;
      font-weight: 700;
    }

    .login-card p {
      color: #64748b;
      font-size: 0.9rem;
    }


  /* Form */
  .form-group {
    text-align: left;
    margin-top: 1rem;
  }

  label {
    font-size: 0.85rem;
    color: #334155;
    display: block;
    margin-bottom: 5px;
  }

  input {
    width: 100%;
    padding: 0.6rem;
    border-radius: 10px;
    border: 1px solid #cbd5f5;
    outline: none;
  }

    input:focus {
      border-color: #3b82f6;
    }

  /* Botón */
  .btn {
    width: 100%;
    margin-top: 1.5rem;
    padding: 0.7rem;
    border: none;
    border-radius: 10px;
    background: linear-gradient(90deg, #3b82f6, #6366f1);
    color: white;
    font-size: 1rem;
    cursor: pointer;
    transition: 0.3s;
  }

    .btn:hover {
      opacity: 0.9;
      transform: translateY(-2px);
    }

  .p-password {
    width: 100% !important;
  }
</style>
