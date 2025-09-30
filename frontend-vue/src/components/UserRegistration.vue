<template>
  <div class="max-w-4xl mx-auto">
    <div class="flex justify-between items-center mb-8 px-6">
      <h1 class="text-4xl font-bold text-gray-800">Sistema Vertrau</h1>
      <div class="space-x-4">
        <button 
          v-if="!showLogin" 
          @click="showLogin = true"
          class="bg-blue-600 text-white px-6 py-3 rounded-md hover:bg-blue-700 text-lg font-semibold shadow-md"
        >
          Login
        </button>
      </div>
    </div>

    <!-- Login Modal -->
    <div v-if="showLogin" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white p-8 rounded-lg shadow-lg max-w-md w-full mx-4">
        <h2 class="text-2xl font-bold mb-6 text-center">Login</h2>
        <form @submit.prevent="$emit('login', loginForm)">
          <div class="mb-4">
            <label class="block text-sm font-medium mb-2">Email</label>
            <input 
              v-model="loginForm.email"
              type="email" 
              required
              class="w-full p-3 border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none"
            >
          </div>
          <div class="mb-6">
            <label class="block text-sm font-medium mb-2">Senha</label>
            <input 
              v-model="loginForm.senha"
              type="password" 
              required
              class="w-full p-3 border border-gray-300 rounded-md focus:border-blue-500 focus:outline-none"
            >
          </div>
          <div class="flex space-x-4">
            <button 
              type="submit"
              class="flex-1 bg-blue-600 text-white p-3 rounded-md hover:bg-blue-700 font-semibold"
            >
              Entrar
            </button>
            <button 
              type="button"
              @click="showLogin = false"
              class="flex-1 bg-gray-300 text-gray-700 p-3 rounded-md hover:bg-gray-400 font-semibold"
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>

    <!-- User Registration Form -->
    <div class="max-w-2xl mx-auto p-8 bg-white rounded-lg shadow-lg border border-gray-200">
      <h1 class="text-3xl font-bold mb-8 text-gray-800 text-center">Cadastro de Usuário</h1>
      
      <div class="flex mb-8">
        <div 
          :class="['flex-1 text-center p-4 text-lg font-semibold rounded-l-md', 
                   step === 1 ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-700']"
        >
          Dados Pessoais
        </div>
        <div 
          :class="['flex-1 text-center p-4 text-lg font-semibold rounded-r-md',
                   step === 2 ? 'bg-blue-600 text-white' : 'bg-gray-200 text-gray-700']"
        >
          Endereço
        </div>
      </div>

      <form @submit.prevent="$emit('submit', form)">
        <!-- Step 1: Personal Data -->
        <div v-if="step === 1" class="space-y-4">
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Nome *</label>
            <input 
              v-model="form.nome"
              required
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Digite seu nome"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Sobrenome *</label>
            <input 
              v-model="form.sobrenome"
              required
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Digite seu sobrenome"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Email *</label>
            <input 
              v-model="form.email"
              type="email"
              required
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Digite seu email"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Senha *</label>
            <input 
              v-model="form.senha"
              type="password"
              required
              minlength="6"
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Digite sua senha"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Gênero *</label>
            <select 
              v-model="form.genero"
              required
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
            >
              <option value="">Selecione</option>
              <option value="1">Masculino</option>
              <option value="2">Feminino</option>
              <option value="3">Outro</option>
            </select>
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Data de Nascimento</label>
            <input 
              v-model="form.dataNascimento"
              type="date"
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
            >
          </div>
          <button 
            type="button"
            @click="step = 2"
            class="w-full bg-blue-600 text-white p-4 rounded-md hover:bg-blue-700 text-lg font-semibold mt-6 shadow-md"
          >
            Próximo
          </button>
        </div>

        <!-- Step 2: Address -->
        <div v-if="step === 2" class="space-y-4">
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">CEP *</label>
            <input 
              v-model="form.endereco.cep"
              @blur="$emit('buscar-cep', form.endereco.cep)"
              required
              maxlength="8"
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Digite o CEP"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Estado *</label>
            <input 
              v-model="form.endereco.estado"
              required
              readonly
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg bg-gray-50 focus:border-blue-500 focus:outline-none"
              placeholder="Estado"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Cidade *</label>
            <input 
              v-model="form.endereco.cidade"
              required
              readonly
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg bg-gray-50 focus:border-blue-500 focus:outline-none"
              placeholder="Cidade"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Logradouro *</label>
            <input 
              v-model="form.endereco.logradouro"
              required
              readonly
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg bg-gray-50 focus:border-blue-500 focus:outline-none"
              placeholder="Logradouro"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Bairro *</label>
            <input 
              v-model="form.endereco.bairro"
              required
              readonly
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg bg-gray-50 focus:border-blue-500 focus:outline-none"
              placeholder="Bairro"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Número *</label>
            <input 
              v-model="form.endereco.numero"
              required
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Digite o número"
            >
          </div>
          <div>
            <label class="block text-base font-semibold mb-2 text-gray-700">Complemento</label>
            <input 
              v-model="form.endereco.complemento"
              class="w-full p-4 border-2 border-gray-300 rounded-md text-lg focus:border-blue-500 focus:outline-none"
              placeholder="Complemento (opcional)"
            >
          </div>
          <div class="flex space-x-4">
            <button 
              type="button"
              @click="step = 1"
              class="flex-1 bg-gray-300 text-gray-700 p-4 rounded-md hover:bg-gray-400 text-lg font-semibold shadow-md"
            >
              Voltar
            </button>
            <button 
              type="submit"
              class="flex-1 bg-green-600 text-white p-4 rounded-md hover:bg-green-700 text-lg font-semibold shadow-md"
            >
              Salvar
            </button>
          </div>
        </div>
      </form>
    </div>

    <!-- Success Modal -->
    <div v-if="showSuccess" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
      <div class="bg-white p-8 rounded-lg shadow-lg max-w-md w-full mx-4">
        <h2 class="text-2xl font-bold mb-4 text-center text-green-600">Usuário Cadastrado!</h2>
        <p class="text-center mb-6">{{ savedUser.nome }} {{ savedUser.sobrenome }} foi cadastrado com sucesso.</p>
        <button 
          @click="$emit('reset')"
          class="w-full bg-blue-600 text-white p-3 rounded-md hover:bg-blue-700 font-semibold"
        >
          Cadastrar Novo Usuário
        </button>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UserRegistration',
  props: {
    showSuccess: Boolean,
    savedUser: Object
  },
  emits: ['login', 'submit', 'buscar-cep', 'reset'],
  data() {
    return {
      step: 1,
      showLogin: false,
      loginForm: {
        email: '',
        senha: ''
      },
      form: {
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
    }
  }
}
</script>
