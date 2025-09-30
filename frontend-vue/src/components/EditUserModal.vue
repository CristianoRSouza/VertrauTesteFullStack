<template>
  <div v-if="show" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div class="bg-white p-6 rounded-lg max-w-2xl w-full mx-4 max-h-screen overflow-y-auto">
      <h2 class="text-2xl font-bold mb-6">Editar Usuário</h2>
      <form @submit.prevent="$emit('save', editForm)">
        <div class="grid grid-cols-2 gap-4">
          <!-- Dados Pessoais -->
          <div class="col-span-2">
            <h3 class="text-lg font-semibold mb-3 text-gray-700">Dados Pessoais</h3>
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Nome *</label>
            <input v-model="editForm.nome" required class="w-full p-3 border rounded-md">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Sobrenome *</label>
            <input v-model="editForm.sobrenome" required class="w-full p-3 border rounded-md">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Email *</label>
            <input v-model="editForm.email" type="email" required class="w-full p-3 border rounded-md">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Gênero *</label>
            <select v-model="editForm.genero" required class="w-full p-3 border rounded-md">
              <option value="">Selecione</option>
              <option value="1">Masculino</option>
              <option value="2">Feminino</option>
              <option value="3">Outro</option>
            </select>
          </div>
          <div class="col-span-2">
            <label class="block text-sm font-medium mb-1">Data de Nascimento</label>
            <input v-model="editForm.dataNascimento" type="date" class="w-full p-3 border rounded-md">
          </div>
          
          <!-- Endereço -->
          <div class="col-span-2 mt-4">
            <h3 class="text-lg font-semibold mb-3 text-gray-700">Endereço</h3>
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">CEP *</label>
            <input v-model="editForm.endereco.cep" @blur="buscarCep" maxlength="8" required class="w-full p-3 border rounded-md">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Estado *</label>
            <input v-model="editForm.endereco.estado" readonly required class="w-full p-3 border rounded-md bg-gray-50">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Cidade *</label>
            <input v-model="editForm.endereco.cidade" readonly required class="w-full p-3 border rounded-md bg-gray-50">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Bairro *</label>
            <input v-model="editForm.endereco.bairro" readonly required class="w-full p-3 border rounded-md bg-gray-50">
          </div>
          <div class="col-span-2">
            <label class="block text-sm font-medium mb-1">Logradouro *</label>
            <input v-model="editForm.endereco.logradouro" readonly required class="w-full p-3 border rounded-md bg-gray-50">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Número *</label>
            <input v-model="editForm.endereco.numero" required class="w-full p-3 border rounded-md">
          </div>
          <div>
            <label class="block text-sm font-medium mb-1">Complemento</label>
            <input v-model="editForm.endereco.complemento" class="w-full p-3 border rounded-md">
          </div>
        </div>
        
        <div class="flex space-x-4 mt-8">
          <button type="submit" class="flex-1 bg-blue-600 text-white p-3 rounded-md hover:bg-blue-700 font-semibold">
            Salvar Alterações
          </button>
          <button type="button" @click="$emit('close')" class="flex-1 bg-gray-300 text-gray-700 p-3 rounded-md hover:bg-gray-400 font-semibold">
            Cancelar
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { cepService } from '../api.js'

export default {
  name: 'EditUserModal',
  props: {
    show: Boolean,
    user: Object
  },
  emits: ['close', 'save'],
  data() {
    return {
      editForm: {
        id: null,
        nome: '',
        sobrenome: '',
        email: '',
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
    }
  },
  watch: {
    user: {
      handler(newUser) {
        if (newUser) {
          this.editForm = {
            id: newUser.id,
            nome: newUser.nome,
            sobrenome: newUser.sobrenome,
            email: newUser.email,
            genero: newUser.genero,
            dataNascimento: newUser.dataNascimento,
            endereco: {
              cep: newUser.endereco?.cep || '',
              estado: newUser.endereco?.estado || '',
              cidade: newUser.endereco?.cidade || '',
              logradouro: newUser.endereco?.logradouro || '',
              bairro: newUser.endereco?.bairro || '',
              numero: newUser.endereco?.numero || '',
              complemento: newUser.endereco?.complemento || ''
            }
          }
        }
      },
      immediate: true
    }
  },
  methods: {
    async buscarCep() {
      if (this.editForm.endereco.cep.length === 8) {
        try {
          const cepData = await cepService.buscarCep(this.editForm.endereco.cep)
          if (!cepData.erro) {
            this.editForm.endereco.estado = cepData.uf
            this.editForm.endereco.cidade = cepData.localidade
            this.editForm.endereco.logradouro = cepData.logradouro
            this.editForm.endereco.bairro = cepData.bairro
          }
        } catch (error) {
          console.error('Erro ao buscar CEP:', error)
        }
      }
    }
  }
}
</script>
