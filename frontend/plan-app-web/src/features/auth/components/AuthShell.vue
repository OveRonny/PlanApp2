<template>
  <main class="auth-page">
    <div class="auth-layout">
      <section class="auth-copy" aria-labelledby="auth-heading">
        <div class="auth-brand">
          <span class="brand-mark">P</span><span>PlanApp AI</span>
        </div>
        <h1 id="auth-heading">Plan smarter.<br />Build with focus.</h1>
        <p>
          Samle ideer, features og oppgaver på ett sted. La AI hjelpe deg fra
          første tanke til ferdig sprint.
        </p>
      </section>
      <section class="auth-card" aria-label="Autentisering">
        <slot name="form" />
      </section>
    </div>
  </main>
</template>
