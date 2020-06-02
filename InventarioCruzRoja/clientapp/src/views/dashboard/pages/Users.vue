<template>
  <v-container
    id="users"
    fluid
    tag="section"
  >
    <v-row justify="center">
      <v-col
        cols="12"
        md="6"
      >
        <form-user :modeEdit="false" />
      </v-col>
    </v-row>
    <v-row>
      <v-col cols="12">
        <users-table />
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
  import FormUser from '../component/FormUser'
  import UsersTable from '../tables/UsersTable'

  export default {
    components: {
      FormUser,
      UsersTable,
    },
  }
</script>

<style lang="sass" scoped>

</style>
