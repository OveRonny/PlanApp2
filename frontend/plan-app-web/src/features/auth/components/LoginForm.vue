<script setup lang="ts">
import { ref } from "vue";
import FormField from "./FormField.vue";
import { authApi, saveSession } from "../api/authApi";
const email = ref("");
const password = ref("");
const error = ref<string | null>(null);
const loading = ref(false);
const emit = defineEmits<{ authenticated: []; register: [] }>();
async function submit() {
  error.value = null;
  loading.value = true;
  try {
    saveSession(await authApi.login(email.value, password.value));
    emit("authenticated");
  } catch (exception) {
    error.value =
      exception instanceof Error ? exception.message : "Innlogging feilet.";
  } finally {
    loading.value = false;
  }
}
</script>
<template>
  <h2>Velkommen tilbake</h2>
  <p class="auth-card-subtitle">Logg inn for å fortsette planleggingen.</p>
  <form class="form-stack" @submit.prevent="submit">
    <FormField
      id="login-email"
      v-model="email"
      label="E-post"
      type="email"
      autocomplete="email"
      placeholder="deg@firma.no"
    /><FormField
      id="login-password"
      v-model="password"
      label="Passord"
      type="password"
      autocomplete="current-password"
      placeholder="••••••••"
    />
    <p v-if="error" class="error-message" role="alert">{{ error }}</p>
    <button class="primary-button" type="submit" :disabled="loading">
      {{ loading ? "Logger inn…" : "Logg inn" }}
    </button>
  </form>
  <p class="secondary-action">
    Har du ikke en konto?
    <button class="text-button" type="button" @click="emit('register')">
      Opprett konto
    </button>
  </p>
</template>
