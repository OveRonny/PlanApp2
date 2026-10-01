<script setup lang="ts">
import { ref } from "vue";
import FormField from "./FormField.vue";
import { authApi } from "../api/authApi";
const email = ref("");
const password = ref("");
const confirmPassword = ref("");
const error = ref<string | null>(null);
const loading = ref(false);
const emit = defineEmits<{ registered: []; login: [] }>();
async function submit() {
  error.value = null;
  if (password.value !== confirmPassword.value) {
    error.value = "Passordene må være like.";
    return;
  }
  loading.value = true;
  try {
    await authApi.register(email.value, password.value);
    emit("registered");
  } catch (exception) {
    error.value =
      exception instanceof Error ? exception.message : "Registrering feilet.";
  } finally {
    loading.value = false;
  }
}
</script>
<template>
  <h2>Opprett konto</h2>
  <p class="auth-card-subtitle">Kom i gang med din første plan.</p>
  <form class="form-stack" @submit.prevent="submit">
    <FormField
      id="register-email"
      v-model="email"
      label="E-post"
      type="email"
      autocomplete="email"
      placeholder="deg@firma.no"
    /><FormField
      id="register-password"
      v-model="password"
      label="Passord"
      type="password"
      autocomplete="new-password"
      placeholder="••••••••"
    />
    <p class="password-hint">Minst 6 tegn, med stor bokstav og et tall.</p>
    <FormField
      id="register-confirm-password"
      v-model="confirmPassword"
      label="Bekreft passord"
      type="password"
      autocomplete="new-password"
      placeholder="••••••••"
    />
    <p v-if="error" class="error-message" role="alert">{{ error }}</p>
    <button class="primary-button" type="submit" :disabled="loading">
      {{ loading ? "Oppretter konto…" : "Opprett konto" }}
    </button>
  </form>
  <p class="secondary-action">
    Har du allerede konto?
    <button class="text-button" type="button" @click="emit('login')">
      Logg inn
    </button>
  </p>
</template>
