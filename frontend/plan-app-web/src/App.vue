<script setup lang="ts">
import { ref } from 'vue'
import AuthShell from './features/auth/components/AuthShell.vue'
import LoginForm from './features/auth/components/LoginForm.vue'
import RegisterForm from './features/auth/components/RegisterForm.vue'
import WorkspacePage from './features/workspaces/components/WorkspacePage.vue'

const isAuthenticated = ref(Boolean(localStorage.getItem('planapp.accessToken')))
const mode = ref<'login' | 'register'>('login')
const notice = ref<string | null>(null)
function switchMode(nextMode: 'login' | 'register') { mode.value = nextMode; notice.value = null }
function handleAuthenticated() { isAuthenticated.value = true }

const mode = ref<'login' | 'register'>('login')
const notice = ref<string | null>(null)
function switchMode(nextMode: 'login' | 'register') { mode.value = nextMode; notice.value = null }
function handleAuthenticated() { notice.value = 'Du er logget inn. Workspace kommer i neste steg.' }
</script>
<template>
  <WorkspacePage v-if="isAuthenticated" />
  <AuthShell v-else>
  <AuthShell>
    <template #form>
      <LoginForm v-if="mode === 'login'" @authenticated="handleAuthenticated" @register="switchMode('register')" />
      <RegisterForm v-else @registered="switchMode('login')" @login="switchMode('login')" />
      <p v-if="notice" class="form-notice" role="status">{{ notice }}</p>
    </template>
  </AuthShell>
</template>
