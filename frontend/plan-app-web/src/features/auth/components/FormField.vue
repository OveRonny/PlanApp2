<script setup lang="ts">
defineProps<{
  id: string;
  label: string;
  type?: string;
  modelValue: string;
  autocomplete?: string;
  placeholder?: string;
}>();
const emit = defineEmits<{ "update:modelValue": [value: string] }>();
</script>
<template>
  <div class="form-field">
    <label :for="id">{{ label }}</label
    ><input
      :id="id"
      :type="type ?? 'text'"
      :value="modelValue"
      :autocomplete="autocomplete"
      :placeholder="placeholder"
      required
      @input="
        emit('update:modelValue', ($event.target as HTMLInputElement).value)
      "
    />
  </div>
</template>
