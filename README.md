# Sistema Vertrau

Sistema de cadastro de usuários com API ASP.NET Core e frontend Vue.js.

## Como executar

```bash
# Com Docker
docker-compose up --build

# Ou separadamente
cd backend/Api && dotnet run
cd frontend-vue && npm run dev
```

## Acesso

- **Frontend**: http://localhost:3000
- **Backend**: http://localhost:5000
- **Swagger**: http://localhost:5000/swagger

## Login padrão

- **Admin**: admin@vertrau.com / admin123
- **Cliente**: cliente@vertrau.com / cliente123

## Funcionalidades

✅ Cadastro de usuários (2 etapas)  
✅ Login/logout com JWT  
✅ Lista de usuários após login  
✅ Editar usuários  
✅ Excluir usuários  
✅ Busca automática de CEP  

## Tecnologias

**Backend**: .NET 8, Entity Framework, JWT, BCrypt  
**Frontend**: Vue.js 3, Tailwind CSS, Axios  
**Banco**: In-Memory (desenvolvimento)
