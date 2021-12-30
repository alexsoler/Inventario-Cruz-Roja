import Vue from 'vue'
import Vuex from 'vuex'

import { auth } from './auth.module'

import EstadosService from '../services/estados.service'
import FabricantesService from '../services/fabricantes.service'
import SedesService from '../services/sedes.service'
import CategoriasService from '../services/categorias.service'

Vue.use(Vuex)

export default new Vuex.Store({
  state: {
    barColor: 'rgba(0, 0, 0, .8), rgba(0, 0, 0, .8)',
    barImage: require('@/assets/img/sidebar-1.jpg'),
    drawer: null,
    estados: [],
    fabricantes: [],
    sedes: [],
    categorias: [],
    meses: ['E', 'F', 'M', 'A', 'M', 'J', 'J', 'A', 'S', 'O', 'N', 'D'],
  },
  getters: {
    estadosGetter (state) {
      return state.estados
    },
    fabricantesGetter (state) {
      return state.fabricantes
    },
    sedesGetter (state) {
      return state.sedes
    },
    categoriasGetter (state) {
      return state.categorias
    },
    mesesGetter (state) {
      return state.meses
    },
  },
  mutations: {
    SET_BAR_IMAGE (state, payload) {
      state.barImage = payload
    },
    SET_DRAWER (state, payload) {
      state.drawer = payload
    },
    SET_ESTADOS (state, payload) {
      state.estados = payload
    },
    SET_FABRICANTES (state, payload) {
      state.fabricantes = payload
    },
    SET_SEDES (state, payload) {
      state.sedes = payload
    },
    SET_CATEGORIAS (state, payload) {
      state.categorias = payload
    },
  },
  actions: {
    async obtenerEstados ({ commit }) {
      let estados = []
      const response = await EstadosService.getAll()
      if (response.status >= 200 && response.status <= 299) {
        estados = response.data
      }
      commit('SET_ESTADOS', estados)
    },
    async obtenerFabricantes ({ commit }) {
      let fabricantes = []
      const response = await FabricantesService.getAll()
      if (response.status >= 200 && response.status <= 299) {
        fabricantes = response.data
      }
      commit('SET_FABRICANTES', fabricantes)
    },
    async obtenerSedes ({ commit }) {
      let sedes = []
      const response = await SedesService.getAll()
      if (response.status >= 200 && response.status <= 299) {
        sedes = response.data
      }
      commit('SET_SEDES', sedes)
    },
    async obtenerCategorias ({ commit }) {
      let categorias = []
      const response = await CategoriasService.getAll()
      if (response.status >= 200 && response.status <= 299) {
        categorias = response.data
      }
      commit('SET_CATEGORIAS', categorias)
    },
  },
  modules: {
    auth,
  },
})
