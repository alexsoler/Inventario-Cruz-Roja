<template>
  <v-app id="inspire">
    <v-main>
      <v-container
        class="fill-height"
        fluid
      >
        <v-row
          align="center"
          justify="center"
        >
          <v-col
            cols="12"
            sm="8"
            md="4"
          >
            <v-card class="elevation-12">
              <v-toolbar
                color="primary"
                dark
                flat
              >
                <v-toolbar-title>Iniciar Sesión</v-toolbar-title>
              </v-toolbar>
              <validation-observer
                ref="observerValidate"
                v-slot="{ invalid }"
              >
                <v-card-text>
                  <v-form
                    id="loginForm"
                    @submit.prevent="handleLogin"
                  >
                    <validation-provider
                      v-slot="{errors}"
                      name="Usuario"
                      rules="required"
                    >
                      <v-text-field
                        v-model="user.username"
                        label="Usuario"
                        name="Username"
                        prepend-icon="mdi-account"
                        type="text"
                        :error-messages="errors"
                      />
                    </validation-provider>

                    <validation-provider
                      v-slot="{errors}"
                      name="Contraseña"
                      rules="required"
                    >
                      <v-text-field
                        id="password"
                        v-model="user.password"
                        label="Contraseña"
                        name="password"
                        prepend-icon="mdi-lock"
                        type="password"
                        :error-messages="errors"
                      />
                    </validation-provider>
                  </v-form>
                </v-card-text>
                <v-card-actions>
                  <v-spacer />
                  <v-btn
                    color="primary"
                    :disabled="loading || invalid"
                    type="submit"
                    form="loginForm"
                  >
                    Entrar
                  </v-btn>
                </v-card-actions>
              </validation-observer>
            </v-card>
          </v-col>
        </v-row>
      </v-container>
    </v-main>
  </v-app>
</template>

<script>
  import User from '../../models/user'
  import { mapGetters } from 'vuex'

  export default {
    name: 'Login',
    data () {
      return {
        user: new User('', ''),
        loading: false,
        message: '',
      }
    },
    computed: {
      ...mapGetters({
        loggedIn: 'auth/loggedIn',
      }),
    },
    created () {
      if (this.loggedIn) {
        this.$router.push('/')
      }
    },
    methods: {
      async handleLogin () {
        this.loading = true
        const isValid = await this.$refs.observerValidate.validate()

        if (!isValid) {
          this.loading = false
          return
        }

        if (this.user.username && this.user.password) {
          this.$store.dispatch('auth/login', this.user).then(
            () => {
              this.$router.push('/')
            },
            error => {
              this.loading = false
              this.message =
                (error.response && error.response.data) ||
                error.message ||
                error.toString()
            },
          )
        }
      },
    },
  }
</script>
