<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref } from 'vue'
import AuthShell from './features/auth/components/AuthShell.vue'
import LoginForm from './features/auth/components/LoginForm.vue'
import RegisterForm from './features/auth/components/RegisterForm.vue'
import WorkspacePage from './features/workspaces/components/WorkspacePage.vue'
import ProjectsPage from './features/projects/components/ProjectsPage.vue'
import { logout } from './features/auth/api/authApi'
import GitHubMembersPage from './features/github/components/GitHubMembersPage.vue'
import { clearGitHubCallback, connectGitHub } from './features/github/api/githubApi'
import ProjectTechnologiesPage from './features/technologies/components/ProjectTechnologiesPage.vue'

const isAuthenticated = ref(Boolean(localStorage.getItem('planapp.accessToken')))
const mode = ref<'login' | 'register'>('login')
const notice = ref<string | null>(null)
const selectedWorkspace = ref<{ id: string; name: string } | null>(null)
const githubProject = ref<{ id: string; name: string } | null>(null)
const githubConnected = ref(localStorage.getItem('planapp.githubConnected') === 'true')
const technologyProject = ref<{ id: string; name: string } | null>(null)

async function handleGitHubCallback() {
  const params = new URLSearchParams(window.location.search)
  const code = params.get('code')
  const projectId = localStorage.getItem('planapp.githubWorkspaceId')
  if (!code || !projectId || !isAuthenticated.value) return
  try {
    await connectGitHub(code)
    localStorage.setItem('planapp.githubConnected', 'true')
    githubConnected.value = true
    clearGitHubCallback()
    localStorage.removeItem('planapp.githubWorkspaceId')
    githubProject.value = { id: projectId, name: 'GitHub project' }
  } catch { clearGitHubCallback() }
}

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

function handleLoggedOut() {
  isAuthenticated.value = false
  selectedWorkspace.value = null
  githubProject.value = null
  githubConnected.value = false
  localStorage.removeItem('planapp.githubConnected')
  mode.value = 'login'
}

onMounted(() => window.addEventListener('planapp:session-expired', handleSessionExpired))
onMounted(() => window.addEventListener('planapp:logged-out', handleLoggedOut))
onMounted(handleGitHubCallback)
onBeforeUnmount(() => {
  window.removeEventListener('planapp:session-expired', handleSessionExpired)
  window.removeEventListener('planapp:logged-out', handleLoggedOut)
})
</script>

<template>
  <ProjectTechnologiesPage v-if="isAuthenticated && technologyProject" :project-id="technologyProject.id" :project-name="technologyProject.name" @back="technologyProject = null" />
  <GitHubMembersPage v-else-if="isAuthenticated && githubProject" :project-id="githubProject.id" :project-name="githubProject.name" :connected="githubConnected" @back="githubProject = null" @connected="githubProject = null" />
  <ProjectsPage v-else-if="isAuthenticated && selectedWorkspace" :workspace-id="selectedWorkspace.id" :workspace-name="selectedWorkspace.name" @back="selectedWorkspace = null" @github="githubProject = $event" @technologies="technologyProject = $event" />
  <WorkspacePage v-else-if="isAuthenticated" @selected="selectedWorkspace = $event" @logout="logout" />
  <AuthShell v-else>
    <template #form>
      <LoginForm v-if="mode === 'login'" @authenticated="handleAuthenticated" @register="switchMode('register')" />
      <RegisterForm v-else @registered="switchMode('login')" @login="switchMode('login')" />
      <p v-if="notice" class="form-notice" role="status">{{ notice }}</p>
    </template>
  </AuthShell>
</template>
