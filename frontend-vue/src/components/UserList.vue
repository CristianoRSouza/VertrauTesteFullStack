<template>
  <div class="max-w-6xl mx-auto p-6">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold text-gray-800">USUÁRIOS CADASTRADOS</h1>
      <button @click="$emit('logout')" class="bg-red-600 text-white px-4 py-2 rounded hover:bg-red-700">
        SAIR
      </button>
    </div>

    <div class="bg-white rounded-lg shadow p-6">
      <h2 class="text-xl mb-4">Total de usuários: {{ users.length }}</h2>
      
      <div v-if="users.length > 0">
        <div v-for="user in users" :key="user.id" class="border-b py-3 flex justify-between items-center">
          <div>
            <div class="font-bold">{{ user.nome }} {{ user.sobrenome }}</div>
            <div class="text-gray-600">{{ user.email }}</div>
            <div class="text-sm text-gray-500">{{ user.endereco?.cidade || 'Sem cidade' }}</div>
          </div>
          <div class="space-x-2">
            <button @click="$emit('edit-user', user)" class="bg-blue-600 text-white px-3 py-1 rounded text-sm hover:bg-blue-700">
              Editar
            </button>
            <button @click="$emit('delete-user', user.id)" class="bg-red-600 text-white px-3 py-1 rounded text-sm hover:bg-red-700">
              Deletar
            </button>
          </div>
        </div>
      </div>
      
      <div v-else class="text-center py-8 text-gray-500">
        Nenhum usuário encontrado
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: 'UserList',
  props: {
    users: {
      type: Array,
      default: () => []
    }
  },
  emits: ['logout', 'edit-user', 'delete-user']
}
</script>
