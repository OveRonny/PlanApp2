<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'
import AuthShell from './features/auth/components/AuthShell.vue'
import LoginForm from './features/auth/components/LoginForm.vue'
import RegisterForm from './features/auth/components/RegisterForm.vue'
import WorkspacePage from './features/workspaces/components/WorkspacePage.vue'
import ProjectsPage from './features/projects/components/ProjectsPage.vue'

const isAuthenticated = ref(Boolean(localStorage.getItem('planapp.accessToken')))
const mode = ref<'login' | 'register'>('login')
const notice = ref<string | null>(null)
const selectedWorkspace = ref<{ id: string; name: string } | null>(null)

function switchMode(nextMode: 'login' | 'register') {
  mode.value = nextMode
  notice.value = null
}

function handleAuthenticated() {
  isAuthenticated.value = true
}

function handleSessionExpired() {
  isAuthenticated.value = false
  mode.value = 'login'
  notice.value = 'Innloggingen din har utløpt. Logg inn på nytt.'
}

onMounted(() => window.addEventListener('planapp:session-expired', handleSessionExpired))
onBeforeUnmount(() => window.removeEventListener('planapp:session-expired', handleSessionExpired))
</script>

<template>
  <ProjectsPage v-if="isAuthenticated && selectedWorkspace" :workspace-id="selectedWorkspace.id" :workspace-name="selectedWorkspace.name" @back="selectedWorkspace = null" />
  <WorkspacePage v-else-if="isAuthenticated" @selected="selectedWorkspace = $event" />
  <AuthShell v-else>
    <template #form>
      <LoginForm v-if="mode === 'login'" @authenticated="handleAuthenticated" @register="switchMode('register')" />
      <RegisterForm v-else @registered="switchMode('login')" @login="switchMode('login')" />
      <p v-if="notice" class="form-notice" role="status">{{ notice }}</p>
    </template>
  </AuthShell>
</template>
