<template>
  <div class="min-h-screen bg-gray-50 py-8">
    <!-- User List Screen (after login) -->
    <UserList 
      v-if="isLoggedIn" 
      :users="users"
      @logout="handleLogout"
      @edit-user="editUser"
      @delete-user="deleteUser"
    />
    
    <!-- Registration/Login Screen -->
    <UserRegistration 
      v-else
      :show-success="showSuccess"
      :saved-user="savedUser"
      @login="login"
      @submit="submitUser"
      @buscar-cep="buscarCep"
      @reset="resetForm"
    />
    
    <!-- Edit User Modal -->
    <EditUserModal
      :show="showEditModal"
      :user="selectedUser"
      @close="closeEditModal"
      @save="updateUser"
    />
  </div>
</template>

<script>
import { authService, usuarioService, cepService } from './api.js'
import UserList from './components/UserList.vue'
import UserRegistration from './components/UserRegistration.vue'
import EditUserModal from './components/EditUserModal.vue'

export default {
  name: 'App',
  components: {
    UserList,
    UserRegistration,
    EditUserModal
  },
  data() {
    return {
      showSuccess: false,
      savedUser: null,
      isLoggedIn: false,
      users: [],
      showEditModal: false,
      selectedUser: null
    }
  },
  mounted() {
    const token = localStorage.getItem('token')
    if (token) {
      this.isLoggedIn = true
      this.loadUsers()
    }
  },
  methods: {
    async login(loginForm) {
      try {
        const response = await authService.login(loginForm)
        localStorage.setItem('token', response.data.token)
        this.isLoggedIn = true
        await this.loadUsers()
      } catch (error) {
        alert('Email ou senha inválidos')
      }
    },
    
    async loadUsers() {
      try {
        const response = await usuarioService.getAll()
        this.users = response.data
      } catch (error) {
        console.error('Erro ao carregar usuários:', error)
        this.users = []
      }
    },
    
    handleLogout() {
      localStorage.removeItem('token')
      this.isLoggedIn = false
      this.users = []
    },
    
    editUser(user) {
      this.selectedUser = user
      this.showEditModal = true
    },
    
    async deleteUser(id) {
      if (confirm('Tem certeza que deseja excluir este usuário?')) {
        try {
          await usuarioService.delete(id)
          await this.loadUsers()
          alert('Usuário excluído com sucesso!')
        } catch (error) {
          alert('Erro ao excluir usuário')
        }
      }
    },
    
    async updateUser(editForm) {
      try {
        const userData = {
          id: editForm.id,
          nome: editForm.nome,
          sobrenome: editForm.sobrenome,
          email: editForm.email,
          senha: "123456",
          role: 0,
          genero: parseInt(editForm.genero),
          dataNascimento: editForm.dataNascimento || null,
          endereco: {
            cep: editForm.endereco.cep,
            estado: editForm.endereco.estado,
            cidade: editForm.endereco.cidade,
            logradouro: editForm.endereco.logradouro,
            bairro: editForm.endereco.bairro,
            numero: editForm.endereco.numero,
            complemento: editForm.endereco.complemento || ""
          }
        }
        
        await usuarioService.update(userData.id, userData)
        this.closeEditModal()
        await this.loadUsers()
        alert('Usuário atualizado com sucesso!')
        
      } catch (error) {
        console.error('Erro ao atualizar:', error)
        alert('Erro ao atualizar usuário: ' + (error.response?.data?.message || error.message))
      }
    },
    
    closeEditModal() {
      this.showEditModal = false
      this.selectedUser = null
    },
    
    async submitUser(form) {
      try {
        const usuario = {
          ...form,
          genero: parseInt(form.genero)
        }
        
        const response = await usuarioService.create(usuario)
        this.savedUser = response.data
        this.showSuccess = true
      } catch (error) {
        alert('Erro ao cadastrar usuário')
      }
    },
    
    async buscarCep(cep) {
      if (cep.length === 8) {
        try {
          const cepData = await cepService.buscarCep(cep)
          if (!cepData.erro) {
            // Emit event back to component would be better, but for simplicity:
            this.$refs.userRegistration?.updateAddress(cepData)
          }
        } catch (error) {
          console.error('Erro ao buscar CEP:', error)
        }
      }
    },
    
    resetForm() {
      this.showSuccess = false
      this.savedUser = null
    }
  }
}
</script>
