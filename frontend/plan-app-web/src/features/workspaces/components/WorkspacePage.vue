<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { createWorkspace, getWorkspaces, type Workspace } from '../api/workspaceApi'

const workspaces = ref<Workspace[]>([])
const name = ref('')
const loading = ref(true)
const saving = ref(false)
const error = ref<string | null>(null)

async function loadWorkspaces() {
  loading.value = true
  error.value = null
  try { workspaces.value = await getWorkspaces() } catch (exception) { error.value = exception instanceof Error ? exception.message : 'Kunne ikke hente workspaces.' } finally { loading.value = false }
}

async function submit() {
  if (!name.value.trim()) return
  saving.value = true
  error.value = null
  try { workspaces.value = [await createWorkspace(name.value.trim()), ...workspaces.value]; name.value = '' } catch (exception) { error.value = exception instanceof Error ? exception.message : 'Kunne ikke opprette workspace.' } finally { saving.value = false }
}

onMounted(loadWorkspaces)
</script>

<template>
  <main class="workspace-page">
    <header class="workspace-header">
      <div><p class="eyebrow">PLANAPP AI</p><h1>Dine workspaces</h1><p class="muted">Samle prosjektene dine på ett sted.</p></div>
      <button class="ghost-button" type="button" @click="loadWorkspaces">Oppdater</button>
    </header>
    <section class="workspace-create" aria-labelledby="create-workspace-heading">
      <div><h2 id="create-workspace-heading">Nytt workspace</h2><p class="muted">Et workspace kan inneholde flere prosjekter.</p></div>
      <form class="create-form" @submit.prevent="submit"><label class="sr-only" for="workspace-name">Navn på workspace</label><input id="workspace-name" v-model="name" placeholder="For eksempel Team Atlas" maxlength="150" required /><button class="primary-button" type="submit" :disabled="saving">{{ saving ? 'Oppretter…' : 'Opprett' }}</button></form>
    </section>
    <p v-if="error" class="workspace-error" role="alert">{{ error }}</p>
    <section aria-live="polite">
      <p v-if="loading" class="muted">Laster workspaces…</p>
      <div v-else-if="workspaces.length" class="workspace-grid"><article v-for="workspace in workspaces" :key="workspace.id" class="workspace-card"><span class="workspace-icon">W</span><div><h3>{{ workspace.name }}</h3><p class="muted">Opprettet {{ new Date(workspace.createdAt).toLocaleDateString('nb-NO') }}</p></div><span class="workspace-arrow">→</span></article></div>
      <div v-else class="empty-state"><h2>Ingen workspaces ennå</h2><p class="muted">Opprett ditt første workspace for å komme i gang.</p></div>
    </section>
  </main>
</template>

<style scoped>
.workspace-page { min-height: 100vh; padding: 48px clamp(20px, 6vw, 88px); color: #f5f7fb; background: #0b1020; }
.workspace-header { display: flex; align-items: end; justify-content: space-between; max-width: 1100px; margin: 0 auto 38px; }
.eyebrow { margin: 0 0 12px; color: #b7f36b; font-size: .75rem; font-weight: 700; letter-spacing: .16em; }
h1, h2, h3, p { margin-top: 0; } h1 { margin-bottom: 10px; font: 600 clamp(2rem, 4vw, 3.4rem)/1 'Space Grotesk', sans-serif; letter-spacing: -.05em; } h2 { margin-bottom: 8px; font: 600 1.25rem 'Space Grotesk', sans-serif; } h3 { margin-bottom: 7px; font-size: 1rem; }
.muted { color: #9da9c3; } .ghost-button { padding: 10px 15px; border: 1px solid #34415f; border-radius: 9px; color: #dce3f2; background: transparent; } .workspace-create { max-width: 1100px; display: flex; align-items: center; justify-content: space-between; gap: 24px; margin: 0 auto 34px; padding: 24px; border: 1px solid #25304a; border-radius: 18px; background: #121a30; }
.create-form { display: flex; gap: 10px; } input { min-width: 250px; padding: 12px 13px; border: 1px solid #34415f; border-radius: 9px; outline: none; color: #f5f7fb; background: #10182d; } input:focus { border-color: #b7f36b; } .primary-button { padding: 12px 18px; border: 0; border-radius: 9px; color: #0b1020; background: #b7f36b; font-weight: 700; } .primary-button:disabled { opacity: .6; }
.workspace-grid { max-width: 1100px; display: grid; grid-template-columns: repeat(auto-fill, minmax(250px, 1fr)); gap: 16px; margin: auto; } .workspace-card { display: flex; align-items: center; gap: 14px; padding: 20px; border: 1px solid #25304a; border-radius: 16px; background: #121a30; } .workspace-card h3 { overflow: hidden; max-width: 190px; text-overflow: ellipsis; white-space: nowrap; } .workspace-icon { display: grid; flex: 0 0 42px; place-items: center; width: 42px; height: 42px; border-radius: 12px; color: #0b1020; background: #b7f36b; font-weight: 800; } .workspace-arrow { margin-left: auto; color: #b7f36b; font-size: 1.4rem; } .workspace-error { max-width: 1100px; margin: 0 auto 20px; color: #ffb8c2; } .empty-state { max-width: 1100px; margin: auto; padding: 60px 20px; border: 1px dashed #34415f; border-radius: 16px; text-align: center; } .sr-only { position: absolute; width: 1px; height: 1px; padding: 0; overflow: hidden; clip: rect(0,0,0,0); white-space: nowrap; border: 0; }
@media (max-width: 700px) { .workspace-header, .workspace-create { align-items: stretch; flex-direction: column; } .create-form { flex-direction: column; } input { min-width: 0; width: 100%; } }
</style>
