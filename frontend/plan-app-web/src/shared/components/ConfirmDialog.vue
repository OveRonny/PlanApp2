<script setup lang="ts">
defineProps<{ open: boolean; title: string; message: string; loading?: boolean }>()
const emit = defineEmits<{ confirm: []; cancel: [] }>()
</script>

<template>
  <Teleport to="body">
    <div v-if="open" class="dialog-backdrop" role="presentation" @click.self="emit('cancel')">
      <section class="dialog" role="alertdialog" aria-modal="true" :aria-labelledby="`${title}-title`">
        <h2 :id="`${title}-title`">{{ title }}</h2>
        <p>{{ message }}</p>
        <div class="dialog-actions">
          <button type="button" class="dialog-button secondary" :disabled="loading" @click="emit('cancel')">Avbryt</button>
          <button type="button" class="dialog-button danger" :disabled="loading" @click="emit('confirm')">{{ loading ? 'Sletter…' : 'Slett workspace' }}</button>
        </div>
      </section>
    </div>
  </Teleport>
</template>

<style scoped>
.dialog-backdrop { position: fixed; z-index: 10; inset: 0; display: grid; place-items: center; padding: 20px; background: rgba(4, 7, 16, .72); }
.dialog { width: min(420px, 100%); padding: 28px; border: 1px solid #34415f; border-radius: 18px; color: #f5f7fb; background: #121a30; box-shadow: 0 24px 80px rgba(0,0,0,.4); }
.dialog h2 { margin: 0 0 10px; font: 600 1.35rem 'Space Grotesk', sans-serif; }
.dialog p { margin: 0; color: #9da9c3; line-height: 1.55; }
.dialog-actions { display: flex; justify-content: flex-end; gap: 10px; margin-top: 26px; }
.dialog-button { padding: 10px 14px; border: 1px solid #34415f; border-radius: 8px; color: #dce3f2; background: transparent; font-weight: 600; }
.dialog-button.danger { border-color: #ff8e9d; color: #0b1020; background: #ff8e9d; }
.dialog-button:disabled { cursor: wait; opacity: .6; }
</style>
