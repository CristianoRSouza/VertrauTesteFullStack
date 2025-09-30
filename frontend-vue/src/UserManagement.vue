<template>
  <div class="max-w-6xl mx-auto p-6">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-800">Gerenciar Usuários</h1>
      <div class="space-x-4">
        <button @click="showCreateModal = true" class="bg-green-600 text-white px-4 py-2 rounded hover:bg-green-700">
          Novo Usuário
        </button>
        <button @click="logout" class="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700">
          Sair
        </button>
      </div>
    </div>

    <!-- Search -->
    <div class="mb-4">
      <input v-model="filtro" @input="loadUsers" placeholder="Buscar usuários..." 
             class="w-full p-3 border rounded-md">
    </div>

    <!-- Users Table -->
    <div class="bg-white rounded-lg shadow overflow-hidden">
      <table class="min-w-full">
        <thead class="bg-gray-50">
          <tr>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Nome</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Email</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Gênero</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Cidade</th>
            <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase">Ações</th>
          </tr>
        </thead>
        <tbody class="divide-y divide-gray-200">
          <tr v-for="user in users" :key="user.id">
            <td class="px-6 py-4">{{ user.nome }} {{ user.sobrenome }}</td>
            <td class="px-6 py-4">{{ user.email }}</td>
            <td class="px-6 py-4">{{ getGeneroText(user.genero) }}</td>
            <td class="px-6 py-4">{{ user.endereco?.cidade || '-' }}</td>
            <td class="px-6 py-4 space-x-2">
              <button @click="editUser(user)" class="text-blue-600 hover:text-blue-800">Editar</button>
              <button @click="deleteUser(user.id)" class="text-red-600 hover:text-red-800">Excluir</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination -->
    <div class="flex justify-between items-center mt-4">
      <button @click="prevPage" :disabled="page === 1" class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50">
        Anterior
      </button>
      <span>Página {{ page }}</span>
      <button @click="nextPage" :disabled="users.length < pageSize" class="px-4 py-2 bg-gray-300 rounded disabled:opacity-50">
        Próxima
      </button>
    </div>

    <!-- Create/Edit Modal -->
    <div v-if="showCreateModal || showEditModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white p-6 rounded-lg max-w-md w-full mx-4 max-h-screen overflow-y-auto">
        <h2 class="text-xl font-bold mb-4">{{ showEditModal ? 'Editar' : 'Criar' }} Usuário</h2>
        <form @submit.prevent="saveUser">
          <div class="space-y-4">
            <input v-model="userForm.nome" placeholder="Nome" required class="w-full p-2 border rounded">
            <input v-model="userForm.sobrenome" placeholder="Sobrenome" required class="w-full p-2 border rounded">
            <input v-model="userForm.email" type="email" placeholder="Email" required class="w-full p-2 border rounded">
            <input v-if="!showEditModal" v-model="userForm.senha" type="password" placeholder="Senha" required class="w-full p-2 border rounded">
            <select v-model="userForm.genero" required class="w-full p-2 border rounded">
              <option value="">Selecione o gênero</option>
              <option value="1">Masculino</option>
              <option value="2">Feminino</option>
              <option value="3">Outro</option>
            </select>
            <input v-model="userForm.dataNascimento" type="date" class="w-full p-2 border rounded">
            <input v-model="userForm.endereco.cep" @blur="buscarCep" placeholder="CEP" maxlength="8" class="w-full p-2 border rounded">
            <input v-model="userForm.endereco.estado" placeholder="Estado" readonly class="w-full p-2 border rounded bg-gray-50">
            <input v-model="userForm.endereco.cidade" placeholder="Cidade" readonly class="w-full p-2 border rounded bg-gray-50">
            <input v-model="userForm.endereco.logradouro" placeholder="Logradouro" readonly class="w-full p-2 border rounded bg-gray-50">
            <input v-model="userForm.endereco.bairro" placeholder="Bairro" readonly class="w-full p-2 border rounded bg-gray-50">
            <input v-model="userForm.endereco.numero" placeholder="Número" required class="w-full p-2 border rounded">
            <input v-model="userForm.endereco.complemento" placeholder="Complemento" class="w-full p-2 border rounded">
          </div>
          <div class="flex space-x-4 mt-6">
            <button type="submit" class="flex-1 bg-blue-600 text-white p-2 rounded hover:bg-blue-700">
              {{ showEditModal ? 'Atualizar' : 'Criar' }}
            </button>
            <button type="button" @click="closeModal" class="flex-1 bg-gray-300 p-2 rounded hover:bg-gray-400">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import { usuarioService, cepService } from './api.js'

export default {
  name: 'UserManagement',
  data() {
    return {
      users: [],
      page: 1,
      pageSize: 10,
      filtro: '',
      showCreateModal: false,
      showEditModal: false,
      userForm: this.getEmptyForm()
    }
  },
  mounted() {
    this.loadUsers()
  },
  methods: {
    getEmptyForm() {
      return {
        nome: '',
        sobrenome: '',
        email: '',
        senha: '',
        genero: '',
        dataNascimento: '',
        endereco: {
          cep: '',
          estado: '',
          cidade: '',
          logradouro: '',
          bairro: '',
          numero: '',
          complemento: ''
        }
      }
    },
    async loadUsers() {
      try {
        const response = await usuarioService.getAll(this.page, this.pageSize, this.filtro)
        this.users = response.data
      } catch (error) {
        alert('Erro ao carregar usuários')
      }
    },
    async saveUser() {
      try {
        const userData = {
          ...this.userForm,
          genero: parseInt(this.userForm.genero)
        }
        
        if (this.showEditModal) {
          await usuarioService.update(userData.id, userData)
        } else {
          await usuarioService.create(userData)
        }
        
        this.closeModal()
        this.loadUsers()
        alert('Usuário salvo com sucesso!')
      } catch (error) {
        alert('Erro ao salvar usuário')
      }
    },
    editUser(user) {
      this.userForm = { ...user }
      this.showEditModal = true
    },
    async deleteUser(id) {
      if (confirm('Tem certeza que deseja excluir este usuário?')) {
        try {
          await usuarioService.delete(id)
          this.loadUsers()
          alert('Usuário excluído com sucesso!')
        } catch (error) {
          alert('Erro ao excluir usuário')
        }
      }
    },
    async buscarCep() {
      if (this.userForm.endereco.cep.length === 8) {
        try {
          const cepData = await cepService.buscarCep(this.userForm.endereco.cep)
          if (!cepData.erro) {
            this.userForm.endereco.estado = cepData.uf
            this.userForm.endereco.cidade = cepData.localidade
            this.userForm.endereco.logradouro = cepData.logradouro
            this.userForm.endereco.bairro = cepData.bairro
          }
        } catch (error) {
          console.error('Erro ao buscar CEP:', error)
        }
      }
    },
    closeModal() {
      this.showCreateModal = false
      this.showEditModal = false
      this.userForm = this.getEmptyForm()
    },
    getGeneroText(genero) {
      const generos = { 1: 'Masculino', 2: 'Feminino', 3: 'Outro' }
      return generos[genero] || '-'
    },
    prevPage() {
      if (this.page > 1) {
        this.page--
        this.loadUsers()
      }
    },
    nextPage() {
      if (this.users.length === this.pageSize) {
        this.page++
        this.loadUsers()
      }
    },
    logout() {
      localStorage.removeItem('token')
      this.$emit('logout')
    }
  }
}
</script>
