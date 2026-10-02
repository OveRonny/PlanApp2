<script setup lang="ts">
import { ref } from 'vue'
import { getCollaborators, getRepositories, startGitHubOAuth, type GitHubCollaborator, type GitHubRepository } from '../api/githubApi'

const props = defineProps<{ projectName: string; projectId?: string; connected?: boolean }>()
const emit = defineEmits<{ back: []; connected: [] }>()
const connected = ref(props.connected ?? localStorage.getItem('planapp.githubConnected') === 'true')
const repository = ref('')
const repositories = ref<GitHubRepository[]>([])
const collaborators = ref<GitHubCollaborator[]>([])
const error = ref<string | null>(null)

async function loadRepositories() { try { repositories.value = await getRepositories() } catch (exception) { error.value = exception instanceof Error ? exception.message : 'Kunne ikke hente repositories.' } }
async function selectRepository() { const selected = repositories.value.find(item => item.fullName === repository.value); if (!selected) return; try { collaborators.value = await getCollaborators(selected) } catch (exception) { error.value = exception instanceof Error ? exception.message : 'Kunne ikke hente medlemmer.' } }

function connectGitHub() {
  if (props.projectId) startGitHubOAuth(props.projectId)
  else connected.value = true
}
if (connected.value) loadRepositories()
function saveMembers() { emit('connected') }
</script>

<template>
  <main class="github-page">
    <header class="github-header">
      <div><button class="back-link" type="button" @click="emit('back')">← Tilbake til prosjekt</button><p class="eyebrow">PROSJEKTINTEGRASJON</p><h1>GitHub-repository</h1><p class="muted">Koble repository og administrer medlemmer for {{ projectName }}.</p></div>
    </header>
    <section class="github-grid">
      <article class="github-panel">
        <span class="github-mark">⌘</span><div><h2>GitHub-konto</h2><p class="muted">Koble til GitHub for å hente repositories og collaborators.</p></div>
        <button class="primary-button github-button" type="button" @click="connectGitHub">{{ connected ? 'Koble til GitHub på nytt' : 'Koble til GitHub' }}</button>
        <p v-if="connected" class="connected-status">✓ GitHub er koblet til</p>
      </article>
      <article class="github-panel">
        <div><p class="eyebrow">REPOSITORY</p><h2>Velg repository</h2><p class="muted">Repositoryet blir koblet til dette prosjektet.</p></div>
        <select v-model="repository" :disabled="!connected" @change="selectRepository"><option value="">{{ repositories.length ? 'Velg repository' : 'Laster repositories…' }}</option><option v-for="item in repositories" :key="item.id" :value="item.fullName">{{ item.fullName }}</option></select><p v-if="error" class="error-text">{{ error }}</p>
      </article>
    </section>
    <section class="member-panel">
      <div class="member-heading"><div><p class="eyebrow">TILGANG</p><h2>Repository-medlemmer</h2></div><button class="primary-button member-save" type="button" :disabled="!connected || !repository" @click="saveMembers">Lagre medlemmer</button></div>
      <p class="muted">Velg hvilke GitHub-collaborators som skal knyttes til prosjektet.</p>
      <div v-if="connected && collaborators.length" class="member-list"><label v-for="member in collaborators" :key="member.login" class="member-row"><input type="checkbox" checked /><span class="avatar">{{ member.login[0].toUpperCase() }}</span><span class="member-name"><strong>{{ member.name || member.login }}</strong><small>@{{ member.login }}</small></span><span class="role">Member</span></label></div>
      <div v-else-if="connected && repository" class="empty-state"><h3>Ingen collaborators funnet</h3><p class="muted">Dette repositoryet har ingen tilgjengelige collaborators.</p></div>
      <div v-else class="empty-state"><h3>Koble til GitHub først</h3><p class="muted">Når kontoen er koblet til, kan du velge repository og medlemmer.</p></div>
    </section>
  </main>
</template>

<style scoped>
.github-page { min-height: 100vh; padding: 48px clamp(20px, 6vw, 88px); color: #f5f7fb; background: #0b1020; }
.error-text { color: #ffb8c2; }
.github-header, .github-grid, .member-panel { max-width: 1100px; margin: 0 auto; }.github-header { margin-bottom: 34px; }.back-link { margin-bottom: 34px; padding: 0; border: 0; color: #b7f36b; background: transparent; }.eyebrow { margin: 0 0 10px; color: #b7f36b; font-size: .75rem; font-weight: 700; letter-spacing: .16em; } h1, h2, h3, p { margin-top: 0; } h1 { margin-bottom: 10px; font: 600 clamp(2rem, 4vw, 3.4rem)/1 'Space Grotesk', sans-serif; letter-spacing: -.05em; } h2 { margin-bottom: 8px; font: 600 1.25rem 'Space Grotesk', sans-serif; }.muted { color: #9da9c3; }
.github-grid { display: grid; grid-template-columns: 1fr 1fr; gap: 16px; margin-bottom: 16px; }.github-panel, .member-panel { padding: 24px; border: 1px solid #25304a; border-radius: 18px; background: #121a30; }.github-panel { display: flex; flex-wrap: wrap; align-items: flex-start; gap: 14px; }.github-panel > div { flex: 1; min-width: 180px; }.github-mark, .avatar { display: grid; place-items: center; width: 42px; height: 42px; border-radius: 12px; color: #0b1020; background: #b7f36b; font-weight: 800; }.github-button { width: auto; margin-top: 8px; }.connected-status { flex-basis: 100%; margin: 8px 0 0; color: #b7f36b; }select { width: 100%; padding: 12px; border: 1px solid #34415f; border-radius: 9px; color: #f5f7fb; background: #10182d; }.member-panel { margin-bottom: 24px; }.member-heading { display: flex; justify-content: space-between; align-items: center; gap: 18px; }.member-save { width: auto; }.member-list { margin-top: 24px; border-top: 1px solid #25304a; }.member-row { display: flex; align-items: center; gap: 14px; padding: 15px 0; border-bottom: 1px solid #25304a; cursor: pointer; }.member-row input { min-width: auto; width: auto; }.member-name { display: grid; flex: 1; gap: 3px; }.member-name small { color: #7785a2; }.role { color: #9da9c3; font-size: .85rem; }.empty-state { margin-top: 22px; padding: 36px 20px; border: 1px dashed #34415f; border-radius: 14px; text-align: center; }
@media (max-width: 700px) { .github-grid { grid-template-columns: 1fr; }.member-heading { align-items: stretch; flex-direction: column; }.member-save { width: 100%; }.github-panel { flex-direction: column; } }
</style>
