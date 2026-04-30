# 📞 CRM Contact Center

Sistema de gestión de clientes, interacciones y seguimientos para contact centers, desarrollado con **ASP.NET Core 8** y **Vue.js 3**.

---

## 🧰 Tecnologías utilizadas

### Backend
| Tecnología | Versión | Descripción |
|---|---|---|
| ASP.NET Core | 8.0 | Framework principal de la API |
| Entity Framework Core | 8.0.11 | ORM para acceso a datos |
| Pomelo.EntityFrameworkCore.MySql | 8.0.2 | Conector MySQL para EF Core |
| JWT Bearer | 8.0.11 | Autenticación con tokens |
| BCrypt.Net-Next | 4.0.3 | Encriptación de contraseñas |
| Swashbuckle (Swagger) | 6.8.1 | Documentación de la API |
| FluentValidation | 11.9.2 | Validación de datos |

### Frontend
| Tecnología | Versión | Descripción |
|---|---|---|
| Vue.js | 3.x | Framework de interfaz de usuario |
| Vite | 5.x | Herramienta de build y desarrollo |
| Vue Router | 4.x | Enrutamiento del frontend |
| Pinia | 2.x | Manejo de estado global |
| Axios | 1.x | Cliente HTTP para llamadas a la API |
| PrimeVue | 4.x | Librería de componentes UI |
| PrimeIcons | 7.x | Íconos |

### Base de datos
| Tecnología | Descripción |
|---|---|
| MySQL 8 | Motor de base de datos principal |

---

## 📁 Estructura del proyecto

```
CrmContactCenter/
├── CrmContactCenter.Server/          ← Backend ASP.NET Core
│   ├── Controllers/                  ← Endpoints de la API
│   │   ├── AuthController.cs
│   │   ├── UsersController.cs
│   │   ├── CustomersController.cs
│   │   ├── AccountsController.cs
│   │   ├── InteractionsController.cs
│   │   ├── FollowUpsController.cs
│   │   └── DashboardController.cs
│   ├── Data/
│   │   └── CrmDbContext.cs           ← Contexto de base de datos
│   ├── DTOs/                         ← Objetos de transferencia de datos
│   │   ├── Auth/
│   │   ├── Users/
│   │   ├── Customers/
│   │   ├── Accounts/
│   │   ├── Interactions/
│   │   └── FollowUps/
│   ├── Models/                       ← Entidades de la base de datos
│   │   ├── Enums/
│   │   │   └── Enums.cs
│   │   ├── User.cs
│   │   ├── Role.cs
│   │   ├── Customer.cs
│   │   ├── Account.cs
│   │   ├── Interaction.cs
│   │   └── FollowUp.cs
│   ├── Services/                     ← Lógica de negocio
│   │   ├── Interfaces/
│   │   │   ├── IAuthService.cs
│   │   │   ├── IUserService.cs
│   │   │   ├── ICustomerService.cs
│   │   │   ├── IAccountService.cs
│   │   │   ├── IInteractionService.cs
│   │   │   └── IFollowUpService.cs
│   │   ├── AuthService.cs
│   │   ├── UserService.cs
│   │   ├── CustomerService.cs
│   │   ├── AccountService.cs
│   │   ├── InteractionService.cs
│   │   └── FollowUpService.cs
│   ├── Middleware/                   ← Middleware personalizado
│   ├── Program.cs                    ← Punto de entrada y configuración
│   └── appsettings.json              ← Configuración de la aplicación
│
└── crmcontactcenter.client/          ← Frontend Vue.js
    ├── src/
    │   ├── assets/
    │   ├── components/
    │   ├── views/                    ← Páginas de la aplicación
    │   │   ├── LoginView.vue
    │   │   ├── DashboardView.vue
    │   │   ├── CustomersView.vue
    │   │   ├── InteractionsView.vue
    │   │   └── FollowUpsView.vue
    │   ├── router/
    │   │   └── index.js              ← Rutas de la aplicación
    │   ├── services/
    │   │   └── api.js                ← Configuración de Axios
    │   ├── stores/
    │   │   └── auth.js               ← Estado de autenticación (Pinia)
    │   ├── App.vue
    │   └── main.js
    ├── vite.config.js
    └── package.json
```

---

## 🗄️ Base de datos

### Tablas principales

```
roles           → Roles del sistema (Administrador, Agente)
users           → Usuarios internos del contact center
customers       → Clientes / deudores
accounts        → Deudas o cuentas asociadas a clientes
interactions    → Registro de llamadas, WhatsApp y emails
follow_ups      → Seguimientos y recordatorios
```

### Relaciones

```
roles      ──── users
users      ──── interactions
users      ──── follow_ups
users      ──── accounts
customers  ──── accounts
customers  ──── interactions
customers  ──── follow_ups
accounts   ──── interactions
interactions ── follow_ups
```

---

## 🚀 Instalación y configuración

### Requisitos previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [Node.js 18+](https://nodejs.org)
- [MySQL 8](https://dev.mysql.com/downloads/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)

### 1. Clonar el repositorio

```bash
git clone https://github.com/tu-usuario/CrmContactCenter.git
cd CrmContactCenter
```

### 2. Configurar la base de datos

Ejecuta el script SQL en MySQL:

```bash
mysql -u root -p < crm_tablas.sql
```

### 3. Configurar la cadena de conexión

Edita `CrmContactCenter.Server/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=crm_contact_center;User=root;Password=TuPassword;"
  },
  "JwtSettings": {
    "SecretKey": "CrmContactCenter2024SecretKeyMuyLargaYSegura!",
    "Issuer": "CrmContactCenter",
    "Audience": "CrmContactCenterUsers",
    "ExpirationHours": 8
  }
}
```

### 4. Insertar datos iniciales

```sql
USE crm_contact_center;

INSERT INTO roles (name, description) VALUES
('Administrador', 'Acceso total al sistema'),
('Agente', 'Gestión de clientes e interacciones');

INSERT INTO users (role_id, first_name, last_name, email, password_hash, is_active)
VALUES (1, 'Admin', 'Sistema', 'admin@crm.com', 'HASH_GENERADO', 1);
```

> ⚠️ Genera el hash de la contraseña llamando a `GET /api/auth/hash/{password}` con Swagger antes de insertar el usuario.

### 5. Instalar dependencias del frontend

```bash
cd crmcontactcenter.client
npm install
```

### 6. Ejecutar el proyecto

Desde Visual Studio presiona **F5** o ejecuta:

```bash
# Backend
dotnet run --project CrmContactCenter.Server

# Frontend (en otra terminal)
cd crmcontactcenter.client
npm run dev
```

---

## 🔐 Autenticación

El sistema usa **JWT (JSON Web Tokens)**. Para acceder a los endpoints protegidos:

1. Llama a `POST /api/auth/login` con email y contraseña
2. Copia el token de la respuesta
3. En Swagger haz clic en **Authorize** e ingresa: `Bearer {token}`

### Roles disponibles

| Rol | Permisos |
|---|---|
| **Administrador** | Acceso total: usuarios, clientes, reportes, eliminar |
| **Agente** | Gestión de clientes, interacciones y seguimientos |

---

## 📡 Endpoints principales

### Autenticación
| Método | Endpoint | Descripción |
|---|---|---|
| POST | `/api/auth/login` | Login de usuario |

### Usuarios
| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/users` | Listar usuarios (Admin) |
| POST | `/api/users` | Crear usuario (Admin) |
| PUT | `/api/users/{id}` | Actualizar usuario |
| PATCH | `/api/users/{id}/toggle-active` | Activar/desactivar |

### Clientes
| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/customers?search=` | Listar con búsqueda |
| GET | `/api/customers/{id}` | Obtener por ID |
| POST | `/api/customers` | Crear cliente |
| PUT | `/api/customers/{id}` | Actualizar cliente |
| DELETE | `/api/customers/{id}` | Eliminar cliente (Admin) |

### Cuentas / Deudas
| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/accounts/customer/{id}` | Deudas de un cliente |
| POST | `/api/accounts` | Registrar deuda |
| PUT | `/api/accounts/{id}` | Actualizar deuda |
| DELETE | `/api/accounts/{id}` | Eliminar deuda (Admin) |

### Interacciones
| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/interactions` | Listar con filtros |
| GET | `/api/interactions/customer/{id}` | Historial del cliente |
| POST | `/api/interactions` | Registrar interacción |
| POST | `/api/interactions/external` | Endpoint externo (bot/WA) |

### Seguimientos
| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/followups` | Listar seguimientos |
| GET | `/api/followups/upcoming` | Próximos 7 días |
| POST | `/api/followups` | Crear seguimiento |
| PATCH | `/api/followups/{id}/status` | Actualizar estado |

### Dashboard
| Método | Endpoint | Descripción |
|---|---|---|
| GET | `/api/dashboard` | Métricas del día |

---

## 📊 Módulos del sistema

### 1. Gestión de usuarios
- Registro de agentes y administradores
- Login con JWT
- Activar / desactivar usuarios
- Control por roles

### 2. Gestión de clientes
- CRUD completo
- Búsqueda por nombre, cédula y teléfono
- Estados: activo / inactivo

### 3. Gestión de cuentas / deudas
- Registro de deudas por cliente
- Estados: pendiente / pagado / vencido
- Control de montos y fechas de vencimiento

### 4. Registro de interacciones
- Canales: llamada, WhatsApp, email
- Resultados: contactado, no responde, promesa de pago, pago realizado, etc.
- Fecha y hora automática
- Relación con cliente, agente y cuenta

### 5. Seguimientos (Follow-ups)
- Recordatorios con fecha programada
- Estados: pendiente / completado / cancelado
- Vista de próximos 7 días

### 6. Dashboard
- Total de clientes
- Deuda pendiente y vencida
- Interacciones del día
- Resultados: contactados, no responden, promesas de pago, pagos realizados

---

## 🌐 Acceso

| Recurso | URL |
|---|---|
| Frontend | https://localhost:63759 |
| Backend API | https://localhost:7165 |
| Swagger | https://localhost:7165/swagger |

---

## 👤 Autor

Desarrollado como proyecto de aprendizaje de **ASP.NET Core 8 + Vue.js 3**.
