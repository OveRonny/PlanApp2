<script setup lang="ts">
import { onMounted, ref } from "vue";
import ConfirmDialog from "../../../shared/components/ConfirmDialog.vue";
import {
  createProject,
  deleteProject,
  getProjects,
  updateProject,
  type Project,
} from "../api/projectApi";

const props = defineProps<{ workspaceId: string; workspaceName: string }>();
const emit = defineEmits<{ back: []; github: [project: Project]; technologies: [project: Project] }>();
const projects = ref<Project[]>([]);
const name = ref("");
const description = ref("");
const editing = ref<Project | null>(null);
const deleting = ref<Project | null>(null);
const loading = ref(true);
const saving = ref(false);
const error = ref<string | null>(null);
async function load() {
  loading.value = true;
  error.value = null;
  try {
    projects.value = await getProjects(props.workspaceId);
  } catch (e) {
    error.value =
      e instanceof Error ? e.message : "Kunne ikke hente prosjekter.";
  } finally {
    loading.value = false;
  }
}
async function create() {
  if (!name.value.trim()) return;
  saving.value = true;
  try {
    projects.value.unshift(
      await createProject(
        props.workspaceId,
        name.value.trim(),
        description.value,
      ),
    );
    name.value = "";
    description.value = "";
  } catch (e) {
    error.value =
      e instanceof Error ? e.message : "Kunne ikke opprette prosjekt.";
  } finally {
    saving.value = false;
  }
}
function startEdit(project: Project) {
  editing.value = { ...project };
  error.value = null;
}
async function saveEdit() {
  if (!editing.value?.name.trim()) return;
  saving.value = true;
  try {
    const updated = await updateProject(
      editing.value.id,
      editing.value.name.trim(),
      editing.value.description ?? "",
    );
    projects.value = projects.value.map((p) =>
      p.id === updated.id ? updated : p,
    );
    editing.value = null;
  } catch (e) {
    error.value =
      e instanceof Error ? e.message : "Kunne ikke oppdatere prosjekt.";
  } finally {
    saving.value = false;
  }
}
async function remove() {
  if (!deleting.value) return;
  try {
    await deleteProject(deleting.value.id);
    projects.value = projects.value.filter((p) => p.id !== deleting.value?.id);
    deleting.value = null;
  } catch (e) {
    error.value =
      e instanceof Error ? e.message : "Kunne ikke slette prosjekt.";
  }
}
onMounted(load);
</script>
<template>
  <main class="workspace-page">
    <header class="workspace-header">
      <div>
        <button class="back-button" type="button" @click="emit('back')">
          ← Workspaces
        </button>
        <p class="eyebrow">{{ workspaceName }}</p>
        <h1>Prosjekter</h1>
        <p class="muted">Planlegg og organiser arbeidet ditt.</p>
      </div>
      <button class="ghost-button" type="button" @click="load">Oppdater</button>
    </header>
    <section class="workspace-create">
      <div>
        <h2>Nytt prosjekt</h2>
        <p class="muted">Start med en tydelig idé.</p>
      </div>
      <form class="create-form" @submit.prevent="create">
        <input
          v-model="name"
          maxlength="150"
          placeholder="Prosjektnavn"
          required
        /><input
          v-model="description"
          placeholder="Beskrivelse (valgfritt)"
        /><button class="primary-button" type="submit" :disabled="saving">
          {{ saving ? "Oppretter…" : "Opprett" }}
        </button>
      </form>
    </section>
    <p v-if="error" class="workspace-error" role="alert">{{ error }}</p>
    <p v-if="loading" class="muted">Laster prosjekter…</p>
    <div v-else-if="projects.length" class="workspace-grid">
      <article
        v-for="project in projects"
        :key="project.id"
        class="workspace-card"
      >
        <span class="workspace-icon">P</span>
        <div class="workspace-info">
          <h3>{{ project.name }}</h3>
          <p class="muted">{{ project.description || "Ingen beskrivelse" }}</p>
        </div>
        <div class="workspace-actions">
          <button class="icon-button" type="button" @click="startEdit(project)">
            Rediger</button
          ><button
            class="icon-button danger"
            type="button"
            @click="deleting = project"
          >
            Slett
          </button>
          <button class="icon-button" type="button" @click.stop="emit('github', project)">GitHub</button>
          <button class="icon-button" type="button" @click.stop="emit('technologies', project)">Teknologier</button>
        </div>
      </article>
    </div>
    <div v-else class="empty-state">
      <h2>Ingen prosjekter ennå</h2>
      <p class="muted">Opprett ditt første prosjekt for å komme i gang.</p>
    </div>
    <div v-if="editing" class="edit-panel">
      <h2>Rediger prosjekt</h2>
      <form class="form-stack" @submit.prevent="saveEdit">
        <input v-model="editing.name" maxlength="150" required /><textarea
          v-model="editing.description"
          rows="3"
          placeholder="Beskrivelse"
        ></textarea>
        <div class="dialog-actions">
          <button class="small-button" type="button" @click="editing = null">
            Avbryt</button
          ><button class="primary-button" type="submit" :disabled="saving">
            Lagre
          </button>
        </div>
      </form>
    </div>
    <ConfirmDialog
      v-if="deleting"
      :open="true"
      title="Slett prosjekt"
      :message="`Er du sikker på at du vil slette «${deleting.name}»?`"
      @cancel="deleting = null"
      @confirm="remove"
    />
  </main>
</template>
<style scoped>
.workspace-page {
  min-height: 100vh;
  padding: 42px clamp(20px, 6vw, 88px);
  color: #f5f7fb;
  background: #0b1020;
}
.workspace-header,
.workspace-create,
.workspace-grid,
.workspace-error,
.empty-state {
  width: min(1100px, 100%);
  margin-right: auto;
  margin-left: auto;
}
.workspace-header {
  display: flex;
  align-items: end;
  justify-content: space-between;
  margin-bottom: 38px;
}
.back-button {
  padding: 0;
  border: 0;
  color: #b7f36b;
  background: none;
  font-weight: 700;
}
.eyebrow {
  margin: 24px 0 10px;
  color: #b7f36b;
  font-size: 0.75rem;
  font-weight: 700;
  letter-spacing: 0.16em;
  text-transform: uppercase;
}
h1,
h2,
h3,
p {
  margin-top: 0;
}
h1 {
  margin-bottom: 10px;
  font:
    600 clamp(2.2rem, 4vw, 3.8rem)/1 "Space Grotesk",
    sans-serif;
  letter-spacing: -0.055em;
}
h2 {
  margin-bottom: 7px;
  font:
    600 1.25rem "Space Grotesk",
    sans-serif;
}
h3 {
  margin-bottom: 8px;
  font-size: 1.05rem;
}
.muted {
  color: #9da9c3;
}
.ghost-button {
  padding: 10px 15px;
  border: 1px solid #34415f;
  border-radius: 9px;
  color: #dce3f2;
  background: transparent;
}
.ghost-button:hover {
  border-color: #b7f36b;
  color: #b7f36b;
}
.workspace-create {
  display: grid;
  grid-template-columns: minmax(180px, 0.7fr) 1.3fr;
  gap: 28px;
  align-items: center;
  margin-bottom: 34px;
  padding: 26px;
  border: 1px solid #25304a;
  border-radius: 18px;
  background: linear-gradient(120deg, #151f39, #121a30);
}
.create-form {
  display: grid;
  grid-template-columns: 1fr 1fr auto;
  gap: 10px;
}
input,
textarea {
  width: 100%;
  padding: 12px 13px;
  border: 1px solid #34415f;
  border-radius: 9px;
  outline: none;
  color: #f5f7fb;
  background: #10182d;
}
input:focus,
textarea:focus {
  border-color: #b7f36b;
  box-shadow: 0 0 0 3px rgba(183, 243, 107, 0.1);
}
textarea {
  resize: vertical;
}
.primary-button {
  padding: 12px 18px;
  border: 0;
  border-radius: 9px;
  color: #0b1020;
  background: #b7f36b;
  font-weight: 700;
}
.primary-button:disabled {
  cursor: wait;
  opacity: 0.6;
}
.workspace-error {
  margin-bottom: 18px;
  color: #ffb8c2;
}
.workspace-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
  gap: 16px;
}
.workspace-card {
  display: flex;
  align-items: flex-start;
  gap: 15px;
  min-height: 132px;
  padding: 21px;
  border: 1px solid #25304a;
  border-radius: 16px;
  background: #121a30;
  transition:
    border-color 0.2s,
    transform 0.2s;
}
.workspace-card:hover {
  border-color: #526487;
  transform: translateY(-2px);
}
.workspace-info {
  min-width: 0;
  flex: 1;
}
.workspace-info h3 {
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}
.workspace-info p {
  display: -webkit-box;
  overflow: hidden;
  -webkit-box-orient: vertical;
  -webkit-line-clamp: 2;
  line-clamp: 2;
  font-size: 0.9rem;
  line-height: 1.4;
}
.workspace-icon {
  display: grid;
  flex: 0 0 42px;
  place-items: center;
  width: 42px;
  height: 42px;
  border-radius: 12px;
  color: #0b1020;
  background: #b7f36b;
  font-weight: 800;
}
.workspace-actions {
  display: flex;
  gap: 6px;
}
.icon-button,
.small-button {
  padding: 6px 8px;
  border: 1px solid #34415f;
  border-radius: 7px;
  color: #dce3f2;
  background: transparent;
  font-size: 0.76rem;
}
.icon-button:hover {
  border-color: #b7f36b;
}
.icon-button.danger:hover {
  border-color: #ff8e9d;
  color: #ffb8c2;
}
.empty-state {
  padding: 66px 20px;
  border: 1px dashed #34415f;
  border-radius: 16px;
  text-align: center;
}
.edit-panel {
  width: min(520px, 100%);
  margin: 28px auto 0;
  padding: 26px;
  border: 1px solid #34415f;
  border-radius: 16px;
  background: #121a30;
}
.edit-panel input {
  width: 100%;
}
.edit-panel .primary-button {
  width: auto;
}
.form-stack {
  display: grid;
  gap: 12px;
}
.dialog-actions {
  display: flex;
  justify-content: flex-end;
  gap: 8px;
  margin-top: 4px;
}
@media (max-width: 760px) {
  .workspace-header {
    align-items: stretch;
    flex-direction: column;
    gap: 22px;
  }
  .workspace-create {
    grid-template-columns: 1fr;
  }
  .create-form {
    grid-template-columns: 1fr;
  }
  .workspace-actions {
    flex-direction: column;
  }
}
</style>
